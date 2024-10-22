using System.ComponentModel.DataAnnotations;

namespace ShopHoNo.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
