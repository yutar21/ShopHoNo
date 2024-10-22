namespace ShopHoNo.Models.ViewModels
{
    public class CreateUserViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SelectedRole { get; set; }

        // Ensure AvailableRoles can be null for initial state
        public IEnumerable<string> AvailableRoles { get; set; } = new List<string>();
    }

}
