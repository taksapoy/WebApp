namespace API.Entities;

public class Connection
{
    public Connection() { } //สำหรับ Entity framework ไว้ใช้ตอนสร้าง schema
    public Connection(string connectionId, string username)
    {
        ConnectionId = connectionId;
        Username = username;
    }

    public string ConnectionId { get; set; }
    public string Username { get; set; }
}