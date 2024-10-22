using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopHoNo.Models.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using ShopHoNo.Models;

namespace ShopHoNo.Controllers
{
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUserModel> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUserModel> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Hiển thị danh sách quyền
        [HttpGet]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        // Tạo quyền mới
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newRole = new IdentityRole { Name = model.RoleName };
                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // Gán quyền cho người dùng
        [Route("User/AssignRole/{userId}")]
        [HttpGet]
        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var model = new AssignRoleViewModel
            {
                UserId = user.Id,
                AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList(),
                AssignedRoles = await _userManager.GetRolesAsync(user) as List<string>
            };
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            // Remove current roles
            var userRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user roles");
                return View(model);
            }

            // Add selected roles
            var addResult = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Index", "User");
        }




        // Xóa quyền
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
