using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : DbContext
{
  public DataContext(DbContextOptions options) : base(options)
  {

  }

 protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserLike>().HasKey(pk => new { pk.SourceUserId, pk.LikedUserId });

        modelBuilder.Entity<UserLike>()
            .HasOne(userlike => userlike.SourceUser)
            .WithMany(appuser => appuser.LikedUsers)
            .HasForeignKey(userlike => userlike.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserLike>()
            .HasOne(userlike => userlike.LikedUser)
            .WithMany(appuser => appuser.LikedByUsers)
            .HasForeignKey(userlike => userlike.LikedUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

  public DbSet<AppUser> Users { get; set; }
  public DbSet<UserLike> Likes { get; set; } 
}