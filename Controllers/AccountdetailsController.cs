using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopHoNo.Data;
using ShopHoNo.Models;

namespace ShopHoNo.Controllers
{
    public class AccountdetailsController : Controller
    {
        private readonly ShopHoNoContext _context;

        public AccountdetailsController(ShopHoNoContext context)
        {
            _context = context;
        }

        // Controllers/AccountdetailsController.cs
        public async Task<IActionResult> Index()
        {
            // Lấy tên người dùng từ session
            var username = HttpContext.Session.GetString("Username");

            // Tìm người dùng trong cơ sở dữ liệu
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound(); // Nếu không tìm thấy người dùng
            }

            return View(user); // Trả về view với thông tin người dùng
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(User user)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật thông tin người dùng trong cơ sở dữ liệu
                var existingUser = await _context.User.FindAsync(user.Username);
                if (existingUser != null)
                {
                    existingUser.Username = user.Username; // Cập nhật tên người dùng
                    existingUser.Email = user.Email; // Cập nhật email

                    // Nếu mật khẩu được nhập, cập nhật mật khẩu
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        existingUser.Password = user.Password; // Mã hóa mật khẩu trước khi lưu
                    }

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();

                    // Cập nhật thông tin trong session
                    HttpContext.Session.SetString("Username", existingUser.Username);
                    HttpContext.Session.SetString("Email", existingUser.Email);

                    return RedirectToAction("Index", "Home"); // Chuyển hướng sau khi cập nhật
                }
            }
            return View(user);
        }
    }
}
