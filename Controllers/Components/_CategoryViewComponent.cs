using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopHoNo.Data;

namespace ShopHoNo.Controllers.Components
{
    [ViewComponent(Name = "_Category")]
    public class _CategoryViewComponent : ViewComponent
    {
        private readonly ShopHoNoContext _context;

        public _CategoryViewComponent(ShopHoNoContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var _category = _context.Category.ToList();
            return View("_Category", _category);

        }
    }
}
