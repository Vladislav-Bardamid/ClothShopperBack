using ClothShopperBack.DAL.Common.VkApiModels;
using ClothShopperBack.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClothShopperBack.DAL;

public class AppDbContext : DbContext
{
    private DbSet<User>? Users { get; set; }
    private DbSet<Album>? Albums { get; set; }
    private DbSet<VkPhoto>? Photos { get; set; }
    private DbSet<UserPhoto>? UserPhotos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}