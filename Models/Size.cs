using System.ComponentModel.DataAnnotations;

namespace ShopHoNo.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        [StringLength(20)]
        [Required]
        public string? SizeName { get; set; }
        [Required]
        public int SizeOrder { get; set; }
        public ICollection<Product>? Products { get; set; } // Quan hệ một-nhiều với Product

    }
}
