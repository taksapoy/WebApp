using api;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace API.SignalR;

[Authorize]
public class MessageHub : Hub
{
    private readonly IHubContext<PresenceHub> _presenceHub;
    private IMessageRepository _messageRepository;
    private IUserRepository _userRepository;
    private IMapper _mapper;


    public MessageHub(IHubContext<PresenceHub> presenceHub,
                        IMessageRepository messageRepository, 
                        IUserRepository userRepository, 
                        IMapper mapper)
    {
        _presenceHub = presenceHub;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }


    public override async Task OnConnectedAsync()
    {
        var callerUsername = Context?.User?.GetUsername();
        var httpContext = Context?.GetHttpContext();
        var otherUsername = httpContext?.Request?.Query["user"].ToString();
        if (Context is null || callerUsername is null || httpContext is null || otherUsername is null) return;

        var groupName = getGroupName(callerUsername, otherUsername);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await addToMessageGroup(groupName);

        var messages = await _messageRepository.GetMessageThread(callerUsername, otherUsername);
        await Clients.Group(groupName).SendAsync("MessageThread", messages);
    }


    private string getGroupName(string caller, string other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }


    public async Task SendMessage(CreateMessageDto createMessageDto)
    { //code ดัดแปลงมาจาก MessagesController.cs -> CreateMessage
        if (createMessageDto.RecipientUsername is null || Context.User is null)
            throw new HubException("not found");

        var username = Context.User.GetUsername();
        if (username is null) return;
        if (username == createMessageDto?.RecipientUsername?.ToLower())
            throw new HubException("can't send to yourself!");

        var sender = await _userRepository.GetUserByUserNameAsync(username);
        var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto!.RecipientUsername);

        if (recipient is null || sender?.UserName is null) throw new HubException("not found");
        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            Content = createMessageDto.Content,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName
        };

        var groupName = getGroupName(sender.UserName, recipient.UserName);
        var group = await _messageRepository.GetMessageGroup(groupName);
        bool isRecipientInGroup = group.Connections.Any(item => item.Username == recipient.UserName);
       if (isRecipientInGroup)
            message.DateRead = DateTime.UtcNow;
        else
        { 
            var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName!);
            if (connections is not null)
                await _presenceHub.Clients.Clients(connections)
                    .SendAsync("NewMessageReceived", new { username = sender.UserName, aka = sender.Aka });
        }
        _messageRepository.AddMessage(message);
        var msdto = _mapper.Map<MessageDto>(message);

        if (await _messageRepository.SaveAllAsync())
            await Clients.Group(groupName).SendAsync("NewMessage", msdto);
    }


    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await removeFromMessageGroup();
        await base.OnDisconnectedAsync(exception);
    }


     private async Task<bool> addToMessageGroup(string groupName)
    {
        if (Context is null || Context.User is null) return false;
        var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

        var group = await _messageRepository.GetMessageGroup(groupName);
        if (group is null)
        {
            group = new MessageGroup(groupName);
            _messageRepository.AddGroup(group);
        }
        group.Connections.Add(connection);
        return await _messageRepository.SaveAllAsync();
    }


    private async Task removeFromMessageGroup()
    {
        if (Context is null) return;
        var connection = await _messageRepository.GetConnection(Context.ConnectionId);
        if (connection is null) return;
        _messageRepository.RemoveConnection(connection);
        await _messageRepository.SaveAllAsync();
    }
}