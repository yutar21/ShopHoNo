using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopHoNo.Models;
using ShopHoNo.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ShopHoNo.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUserModel> _userManager; // Thay đổi đây

        public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Hiển thị danh sách người dùng
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var userRoles = users.Select(user => new
            {
                User = user,
                Roles = _userManager.GetRolesAsync(user).Result // Fetch roles asynchronously
            }).ToList();

            return View(userRoles);
        }


        // Tạo người dùng mới
        [HttpGet]
        public IActionResult CreateUser()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList(); // Get available roles
            var model = new CreateUserViewModel { AvailableRoles = roles };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUserModel { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign role if selected
                    if (!string.IsNullOrEmpty(model.SelectedRole))
                    {
                        await _userManager.AddToRoleAsync(user, model.SelectedRole);
                    }
                    // Redirect to the Index action after successful creation
                    return RedirectToAction("Index");
                }

                // Add errors to the model state
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // Populate roles again if ModelState is invalid
            model.AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(model); // Return the view with model to show errors
        }




        // Chỉnh sửa người dùng
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var assignedRoles = await _userManager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                AssignedRole = assignedRoles.FirstOrDefault(),
                AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList() // Get available roles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null) return NotFound();

                user.UserName = model.Username;
                user.Email = model.Email;

                // Update user roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Count > 0)
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                if (!string.IsNullOrEmpty(model.AssignedRole))
                {
                    await _userManager.AddToRoleAsync(user, model.AssignedRole); // Assign new role
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            model.AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(model);
        }


        // Xóa người dùng
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
