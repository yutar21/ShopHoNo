using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopHoNo.Models;

namespace ShopHoNo.Data
{
    public class ShopHoNoContext : IdentityDbContext<AppUserModel>
    {
        public ShopHoNoContext (DbContextOptions<ShopHoNoContext> options)
            : base(options)
        {
        }

        public DbSet<ShopHoNo.Models.User> User { get; set; } = default!;
        public DbSet<ShopHoNo.Models.Brand> Brand { get; set; } = default!;
        public DbSet<ShopHoNo.Models.Category> Category { get; set; } = default!;
        public DbSet<ShopHoNo.Models.Product> Product { get; set; } = default!;
        public DbSet<ShopHoNo.Models.Size> Size { get; set; } = default!;

		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<OrderStatus> OrderStatuses { get; set; } // Add this line

		// protected override void OnModelCreating(ModelBuilder modelBuilder)
		// {
		// 	modelBuilder.Entity<Order>()
		// 		.HasOne(o => o.OrderStatus)
		// 		.WithMany()
		// 		.HasForeignKey(o => o.OrderStatusId);

		// 	// Seed initial data for OrderStatus
		// 	modelBuilder.Entity<OrderStatus>().HasData(
		// 		new OrderStatus { Id = 1, StatusName = "Pending" },
		// 		new OrderStatus { Id = 2, StatusName = "Processing" },
		// 		new OrderStatus { Id = 3, StatusName = "Completed" },
		// 		new OrderStatus { Id = 4, StatusName = "Cancelled" }
		// 	);
		// }
	}
}
