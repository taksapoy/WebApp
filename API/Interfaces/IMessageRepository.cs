using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IMessageRepository
{
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message> GetMessage(int id);
    Task<IEnumerable<MessageDto>> GetMessageThread(string senderUserName, string recipientUserName);
    Task<PageList<MessageDto>> GetUserMessages(MessageParams messageParams);
    Task<bool> SaveAllAsync();
}