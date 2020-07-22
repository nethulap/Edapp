using System;
using Edapp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Edapp.Data
{
    public class EdappContext : DbContext
    {
        private static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public EdappContext(DbContextOptions<EdappContext> options, IConfiguration configuration) : base(options)
        {
        }

        public void LoadUsers()
        {
            var user1 = new User
            {
                Id = 1,
                Name = "Albert Einstein",
                Email = "albert@gmail.com",
                Phone = "+61419876543"
            };

            var user2 = new User
            {
                Id = 2,
                Name = "Isaac Newton",
                Email = "isaac@gmail.com",
                Phone = "+61412314151"
            };

            var user3 = new User
            {
                Id = 3,
                Name = "Richard Feynman",
                Email = "richard@gmail.com",
                Phone = "+61412345678"
            };

            User.Add(user1);
            User.Add(user2);
            User.Add(user3);
            SaveChanges();
        }

        public void LoadItems()
        {
            var item1 = new Item
            {
                Id = 1,
                ItemNumber = 2345,
                Description = "Iphone X 250 GB Space Gray",
                CreatedDate = DateTime.Now,
                AuctionStartDate = DateTime.Now,
                AuctionEndDate = DateTime.Today.AddDays(1)
            };

            var item2 = new Item
            {
                Id = 2,
                ItemNumber = 2346,
                Description = "Iphone X 512 GB White",
                CreatedDate = DateTime.Now,
                AuctionStartDate = DateTime.Now,
                AuctionEndDate = DateTime.Today.AddDays(1)
            };

            var item3 = new Item
            {
                Id = 3,
                ItemNumber = 2347,
                Description = "Iphone X 64 GB Gray",
                CreatedDate = DateTime.Now,
                AuctionStartDate = DateTime.Now,
                AuctionEndDate = DateTime.Today.AddDays(1)
            };

            var item4 = new Item
            {
                Id = 4,
                ItemNumber = 2348,
                Description = "Iphone X 128 GB Red",
                CreatedDate = DateTime.Now,
                AuctionStartDate = DateTime.Now,
                AuctionEndDate = DateTime.Today.AddDays(1)
            };

            var item5 = new Item
            {
                Id = 5,
                ItemNumber = 2349,
                Description = "Iphone X 250 GB",
                CreatedDate = DateTime.Now,
                AuctionStartDate = DateTime.Now,
                AuctionEndDate = DateTime.Today.AddDays(1)
            };

            Item.Add(item1);
            Item.Add(item2);
            Item.Add(item3);
            Item.Add(item4);
            Item.Add(item5);
            SaveChanges();
        }

        public DbSet<User> User { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Bid> Bid { get; set; }
    }
}
