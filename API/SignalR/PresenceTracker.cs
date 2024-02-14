namespace API.SignalR;

public class PresenceTracker
{
    private static readonly Dictionary<string, List<string>> OnlineUsers = new();

    public Task UserConnected(string username, string connectionId)
    {
        lock (OnlineUsers) //lock เพื่อป้องกัน race condition, อาจเกิดปัญหาคอขวด
        {
            if (OnlineUsers.ContainsKey(username))
                OnlineUsers[username].Add(connectionId);
            else
                OnlineUsers.Add(username, new List<string> { connectionId });
        }
        return Task.CompletedTask;
    }
    public Task UserDisconnected(string username, string connectionId)
    {
        lock (OnlineUsers)
        {
            if (OnlineUsers.ContainsKey(username))
                OnlineUsers[username].Remove(connectionId);
            if (OnlineUsers[username].Count < 1)//มากกว่า 1 เมื่อ connect มากกว่า 1 browser
                OnlineUsers.Remove(username);
        }
        return Task.CompletedTask;
    }
    public Task<string[]> GetOnlineUsers()
    {
        string[] users;
        lock (OnlineUsers)
        {
            users = OnlineUsers.OrderBy(item => item.Key).Select(item => item.Key).ToArray();
        }
        return Task.FromResult(users);
    }
}