using System.ComponentModel.DataAnnotations;

namespace ShopHoNo.Models.ViewModels
{
    public class LoginViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "UserName là bắt buộc.")]
        public string? Username { get; set; } // Tên đăng nhập

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string? Password { get; set; } // Mật khẩu (nên mã hóa trong thực tế)

    }
}
