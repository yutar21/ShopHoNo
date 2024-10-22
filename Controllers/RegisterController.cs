using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopHoNo.Data;
using ShopHoNo.Models;

namespace ShopHoNo.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ShopHoNoContext _context;

        public RegisterController(ShopHoNoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCustomer(User user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem email đã tồn tại chưa
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View("Index", user);
                }

                // Lưu người dùng vào cơ sở dữ liệu
                _context.User.Add(user);
                await _context.SaveChangesAsync();

                // Chuyển hướng đến trang đăng nhập hoặc trang chính
                return RedirectToAction("Index", "Register");
            }

            return View("Index", user); // Trả về trang đăng ký với thông tin đã nhập
        }
    }
}