using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopHoNo.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; } // Primary key

        public string? image {  get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int quantity {  get; set; }
		public string? Size { get; set; }
        public string? UserId { get; set; } // Khóa ngoại đến AspNetUsers
        public AppUserModel? User { get; set; } // Điều hướng đến User
        public decimal SumPrice => Price * quantity;
	}
}