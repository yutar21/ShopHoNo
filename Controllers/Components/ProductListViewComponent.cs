
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopHoNo.Data;
using ShopHoNo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopHoNo.Components
{
    [ViewComponent(Name = "ProductList")]
    public class ProductListViewComponent : ViewComponent
    {
        private readonly ShopHoNoContext _context;

        public ProductListViewComponent(ShopHoNoContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _context.Product
                .Include(p => p.Size)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync();

            return View("_ProductList", products); // Trả về view với danh sách sản phẩm
        }
    }
}