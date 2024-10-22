using System.Collections.Generic;

namespace ShopHoNo.Models.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public List<string> AvailableRoles { get; set; } = new List<string>();
        public string AssignedRole { get; set; }
    }
}
