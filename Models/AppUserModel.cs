using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ShopHoNo.Models
{
    public class AppUserModel : IdentityUser
    {
        // Thêm danh sách các đơn hàng của người dùng
        public List<Order> Orders { get; set; } = new List<Order>(); // Khóa ngoại tới Order
    }
}
