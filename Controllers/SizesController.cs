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
    public class SizesController : Controller
    {
        private readonly ShopHoNoContext _context;

        public SizesController(ShopHoNoContext context)
        {
            _context = context;
        }

        // GET: Sizes
        public async Task<IActionResult> Index()
        {
            var sizes = await _context.Size.ToListAsync();
            return View(sizes);
        }

        // GET: Sizes/Search
        public async Task<IActionResult> Search(string searchTerm)
        {
            ViewData["ActivePage"] = "Size"; // Ensure the active page is set
            var sizes = await _context.Size
                .Where(s => s.SizeName.Contains(searchTerm)) // Filter by size name
                .ToListAsync();

            return View("Index", sizes); // Return the filtered size list to the Index view
        }
		// GET: Sizes/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Sizes/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Size size)
		{
			if (ModelState.IsValid)
			{
				_context.Add(size);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index)); // Redirect to the index after creation
			}
			return View(size); // Return the form with errors
		}

		// GET: Sizes/Edit/{id}
		public async Task<IActionResult> Edit(int id)
		{
			var size = await _context.Size.FindAsync(id);
			if (size == null)
			{
				return NotFound();
			}
			return View(size);
		}

		// POST: Sizes/Edit/{id}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Size size)
		{
			if (id != size.SizeId) // Ensure the ID matches
			{
				return BadRequest();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(size);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SizeExists(size.SizeId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index)); // Redirect to the index after editing
			}
			return View(size); // Return the form with errors
		}

		// Helper method to check if size exists
		private bool SizeExists(int id)
		{
			return _context.Size.Any(e => e.SizeId == id);
		}

		// GET: Sizes/Delete/{id}
		public async Task<IActionResult> Delete(int id)
		{
			var size = await _context.Size.FindAsync(id);
			if (size == null)
			{
				return NotFound();
			}
			return View(size);
		}

		// POST: Sizes/Delete/{id}
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var size = await _context.Size.FindAsync(id);
			if (size != null)
			{
				_context.Size.Remove(size);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index)); // Redirect to the index after deletion
		}
		// GET: Sizes/Details/{id}
		public async Task<IActionResult> Details(int id)
		{
			var size = await _context.Size.FindAsync(id);
			if (size == null)
			{ 
				return NotFound();
			}
			return View(size);
		}
	}
}
