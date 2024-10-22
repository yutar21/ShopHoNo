using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopHoNo.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        public string? Notes { get; set; } // Optional

        public DateTime DateTime { get; set; }

        public int OrderStatusId { get; set; } // Foreign key for OrderStatus
        public OrderStatus? OrderStatus { get; set; } // Navigation property

        public List<OrderItem>? OrderItems { get; set; } // Navigation property for order items

        public string? UserId { get; set; } // Khóa ngoại đến AspNetUsers
        public AppUserModel? User { get; set; } // Điều hướng đến User

        // Tính tổng giá tiền của đơn hàng
        public decimal TotalPrice => OrderItems?.Sum(item => item.Price * item.Quantity) ?? 0;
    }
}
