using System.ComponentModel.DataAnnotations;

namespace ShopHoNo.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } // Khóa chính

        [Required]
        [StringLength(100)]
        public string? CategoryName { get; set; } // Tên danh mục

        [Required]
        public int CategoryOrder { get; set; }

        public ICollection<Product>? Products { get; set; } // Quan hệ một-nhiều với Product
    }
}
