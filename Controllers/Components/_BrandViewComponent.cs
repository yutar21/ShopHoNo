using Microsoft.AspNetCore.Mvc;
using ShopHoNo.Data;

namespace ShopHoNo.Controllers.Components
{
    [ViewComponent(Name = "_Brand")]
    public class _BrandViewComponent : ViewComponent
    {
        private readonly ShopHoNoContext _context;

        public _BrandViewComponent(ShopHoNoContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var _Brand = _context.Brand.ToList();
            return View("_Brand", _Brand);

        }
    }
}
