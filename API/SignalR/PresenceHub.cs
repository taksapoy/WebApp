using api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class PresenceHub : Hub
{
    private readonly PresenceTracker _presenceTracker;
    public PresenceHub(PresenceTracker presenceTracker)
    {
        _presenceTracker = presenceTracker;
    }


    public override async Task OnConnectedAsync()
    {
        var username = Context?.User?.GetUsername();
        if (username is null || Context is null) return;

        await _presenceTracker.UserConnected(username, Context.ConnectionId);
        await Clients.Others.SendAsync("UserOnline", username);

        var onlineUsers = await _presenceTracker.GetOnlineUsers();
        await Clients.All.SendAsync("OnlineUsers", onlineUsers);
    }


    public override async Task OnDisconnectedAsync(Exception exception) {
        var username = Context?.User?.GetUsername();
        if (username is null || Context is null) return;
        await _presenceTracker.UserDisconnected(username, Context.ConnectionId);
        await Clients.Others.SendAsync("UserOffline", username);
        var onlineUsers = await _presenceTracker.GetOnlineUsers();
        await Clients.All.SendAsync("OnlineUsers", onlineUsers);
        await base.OnDisconnectedAsync(exception);
    }
}