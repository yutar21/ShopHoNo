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
    [Authorize(Roles = "admin")]
    public class CategoriesController : Controller
    {
        private readonly ShopHoNoContext _context;

        public CategoriesController(ShopHoNoContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Category.ToListAsync();
            return View(categories);
        }

        // GET: Categories/Search
        public async Task<IActionResult> Search(string searchTerm)
        {
            ViewData["ActivePage"] = "Category"; // Ensure the active page is set
            var categories = await _context.Category
                .Where(c => c.CategoryName.Contains(searchTerm)) // Filter by category name
                .ToListAsync();

            return View("Index", categories); // Return the filtered category list to the Index view
        }
		// GET: Categories/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Categories/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Category category)
		{
			if (ModelState.IsValid)
			{
				_context.Add(category);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index)); // Redirect to the index after creation
			}
			return View(category); // Return the form with errors
		}

		// GET: Categories/Edit/{id}
		public async Task<IActionResult> Edit(int id)
		{
			var category = await _context.Category.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		// POST: Categories/Edit/{id}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Category category)
		{
			if (id != category.CategoryId)
			{
				return BadRequest();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(category);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CategoryExists(category.CategoryId))
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
			return View(category);
		}

		// Helper method to check if category exists
		private bool CategoryExists(int id)
		{
			return _context.Category.Any(e => e.CategoryId == id);
		}

		// GET: Categories/Delete/{id}
		public async Task<IActionResult> Delete(int id)
		{
			var category = await _context.Category.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		// POST: Categories/Delete/{id}
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var category = await _context.Category.FindAsync(id);
			if (category != null)
			{
				_context.Category.Remove(category);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}

		// GET: Categories/Details/{id}
		public async Task<IActionResult> Details(int id)
		{
			var category = await _context.Category.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}
	}
}
