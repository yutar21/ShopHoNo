using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopHoNo.Data;
using ShopHoNo.Models;

namespace ShopHoNo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShopHoNoContext _context;

        public ProductsController(ShopHoNoContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Product";
            var shopHoNoContext = _context.Product.Include(p => p.Size).Include(p => p.Brand).Include(p => p.Category);
            return View(await shopHoNoContext.ToListAsync());
        }
        public async Task<IActionResult> FilterProductsByCategory(int categoryId) // Đảm bảo tham số đúng
        {
            var products = await _context.Product
                .Where(p => p.CategoryId == categoryId) // Lọc theo CategoryId
                .Include(p => p.Size)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync();

            return PartialView("Components/ProductList/_ProductList", products); // Trả về PartialView
        }
        public async Task<IActionResult> FilterProductsByBrand(int BrandId) // Đảm bảo tham số đúng
        {
            var products = await _context.Product
                .Where(p => p.BrandId == BrandId) // Lọc theo Brand
                .Include(p => p.Size)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync();

            return PartialView("Components/ProductList/_ProductList", products); // Trả về PartialView
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Size)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName");
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandName");
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Image,Description,Quantity,SizeId,CategoryId,BrandId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName", product.SizeId);
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandName", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName", product.SizeId);
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandName", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Image,Description,Quantity,SizeId,CategoryId,BrandId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName", product.SizeId);
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandName", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Size)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        public async Task<IActionResult> GetAllProducts() // Phương thức mới
        {
            var products = await _context.Product
                .Include(p => p.Size)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync();

            return PartialView("Components/ProductList/_ProductList", products); // Trả về PartialView
        }

        public async Task<IActionResult> SearchProducts(string searchTerm) // Phương thức tìm kiếm
        {
            var products = await _context.Product
                .Where(p => p.Name.Contains(searchTerm)) // Tìm kiếm theo tên sản phẩm
                .Include(p => p.Size)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync();

            return PartialView("Components/ProductList/_ProductList", products); // Trả về PartialView
        }

        // Add this method to handle product search
        public async Task<IActionResult> Search(string searchTerm)
        {
            ViewData["ActivePage"] = "Product"; // Ensure the active page is set
            var products = await _context.Product
                .Include(p => p.Size)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(searchTerm)) // Filter by product name
                .ToListAsync();

            return View("Index", products); // Return the filtered product list to the Index view
        }

        // GET: Products/ProductDetail/5
        public async Task<IActionResult> ProductDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Size)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product); // Return the product details view
        }

    }
}
