using System.ComponentModel.DataAnnotations;

namespace ShopHoNo.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }
        [Required]
        [StringLength(20)]
        public string? BrandName { get; set; }
        [Required]
        public int BrandOrder { get; set; }

        public ICollection<Product>? Products { get; set; } // Quan hệ một-nhiều với Product

    }
}
