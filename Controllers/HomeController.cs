using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopHoNo.Data;
using ShopHoNo.Models;
using System.Diagnostics;

namespace ShopHoNo.Controllers
{
    public class HomeController : Controller
    {

		private readonly ShopHoNoContext _context;

		public HomeController(ShopHoNoContext context)
		{
			_context = context;
		}

        public IActionResult Index()
        {
			var _Product = _context.Product.Include(p => p.Size).Include(p => p.Brand).Include(p => p.Category);
			return View(_Product.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
