using ClothShopperBack.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClothShopperBack.DAL;

public class AppDbContext : IdentityDbContext<User, Role, int>
{
    public DbSet<Album> Albums { get; set; }
    public DbSet<Cloth> Clothes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderList> OrderLists { get; set; }

    protected AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().HasKey(x => new { x.UserId, x.ClothId });
        modelBuilder.Entity<User>().HasMany(x => x.Orders).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        modelBuilder.Entity<Cloth>().HasMany(x => x.Orders).WithOne(x => x.Cloth).HasForeignKey(x => x.ClothId);

        InitData(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void InitData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>().HasData(
            new Album()
            {
                Id = 1,
                Title = "Галина (Открытый)",
                VkAlbumId = 263258790,
                VkOwnerId = 174012088
            },
            new Album()
            {
                Id = 2,
                Title = "Галина (Закрытый)",
                VkAlbumId = 263431528,
                VkOwnerId = 174012088
            }
        );
    }
}