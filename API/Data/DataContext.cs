using API.Entities;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    //snippet: typing "prop" then press tap
    public DbSet<AppUser> Users { get; set; }
}