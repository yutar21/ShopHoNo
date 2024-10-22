using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopHoNo.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; } // Khóa chính

        [Required]
        [StringLength(100)]
        public string? Name { get; set; } // Tên sản phẩm

        [Required]
        public decimal Price { get; set; } // Giá sản phẩm

        [Required]
        [StringLength(50)]
        public string? Image { get; set; } // Ảnh sản phẩm

        [StringLength(100)]
        public string? Description { get; set; } // Mô tả sản phẩm

        [Required]
        public int? Quantity { get; set; } // Số lượng 

        [ForeignKey("SizeId")]
        public int SizeId { get; set; }
        public Size? Size { get; set; } // Khóa ngoại đến Size

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; } // Khóa ngoại đến Category

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }
        public Brand? Brand { get; set; } // Khóa ngoại đến Brand

        public ICollection<CartItem>? CartItems { get; set; } // Quan hệ một-nhiều với CartItem
    }
}
