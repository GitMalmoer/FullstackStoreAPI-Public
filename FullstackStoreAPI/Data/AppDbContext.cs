using FullstackStoreAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FullstackStoreAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    Id = 1,
                    Name = "Orange Polish",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://nailsstorage.blob.core.windows.net/fullstack-store/bottle2.png",
                    Price = 7.99,
                    Category = "Polishers",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 2,
                    Name = "Yellow Polish",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://nailsstorage.blob.core.windows.net/fullstack-store/bottle1.png",
                    Price = 8.99,
                    Category = "Polishers",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 3,
                    Name = "Blue Polish",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://nailsstorage.blob.core.windows.net/fullstack-store/bottle3.png",
                    Price = 8.99,
                    Category = "Polishers",
                    SpecialTag = "Best Seller"
                }, new MenuItem
                {
                    Id = 4,
                    Name = "Pink Polish",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://nailsstorage.blob.core.windows.net/fullstack-store/bottle4.png",
                    Price = 10.99,
                    Category = "Polishers",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 5,
                    Name = "Bundle",
                    Description = "Bundle of most common manicure tools",
                    Image = "https://nailsstorage.blob.core.windows.net/fullstack-store/bundle1.png",
                    Price = 12.99,
                    Category = "Equipment",
                    SpecialTag = "Top Rated"
                }, new MenuItem
                {
                    Id = 6,
                    Name = "Nail Acetone",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://nailsstorage.blob.core.windows.net/fullstack-store/cosmetic2.png",
                    Price = 11.99,
                    Category = "Cosmetics",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 7,
                    Name = "Basic File",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://nailsstorage.blob.core.windows.net/fullstack-store/File1.png",
                    Price = 13.99,
                    Category = "Files",
                    SpecialTag = "Most popular"
                }, new MenuItem
                {
                    Id = 8,
                    Name = "Square File",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://nailsstorage.blob.core.windows.net/fullstack-store/SquareFile.png",
                    Price = 4.99,
                    Category = "Files",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 9,
                    Name = "Nail Serum",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://nailsstorage.blob.core.windows.net/fullstack-store/serum1.png",
                    Price = 4.99,
                    Category = "Cosmetics",
                    SpecialTag = "Top Rated"
                }, new MenuItem
                {
                    Id = 10,
                    Name = "UV Lamp",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://nailsstorage.blob.core.windows.net/fullstack-store/lamp.png",
                    Price = 3.99,
                    Category = "Equipment",
                    SpecialTag = "Top Rated"
                }
            );


        }
    }
}
