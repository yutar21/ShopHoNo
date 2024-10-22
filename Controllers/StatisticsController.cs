using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace ShopHoNo.Controllers
{
    [Authorize(Roles ="admin")]
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
