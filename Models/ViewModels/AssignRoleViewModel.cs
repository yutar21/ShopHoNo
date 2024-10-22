namespace ShopHoNo.Models.ViewModels
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; }
        public List<string> AvailableRoles { get; set; }
        public List<string> AssignedRoles { get; set; }
        public List<string> SelectedRoles { get; set; } = new List<string>(); // Initialize the list
    }
}
