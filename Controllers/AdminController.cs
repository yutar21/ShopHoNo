using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopHoNo.Data;
using Microsoft.EntityFrameworkCore; // Thêm dòng này

namespace ShopHoNo.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ShopHoNoContext _context;

        public AdminController(ShopHoNoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Home"; // Thiết lập trang hiện tại
            var productCount = await _context.Product.CountAsync();
            var categoryCount = await _context.Category.CountAsync();
            var brandCount = await _context.Brand.CountAsync();
            var orderCount = await _context.Orders.CountAsync();
            ViewBag.BrandCount = brandCount;
            ViewBag.ProductCount = productCount;
            ViewBag.CategoryCount = categoryCount;
            ViewBag.OrderCount = orderCount;
            return View();
        }
    }
}
