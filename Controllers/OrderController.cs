using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopHoNo.Controllers.KEY;
using ShopHoNo.Controllers.Session;
using ShopHoNo.Data;
using ShopHoNo.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShopHoNo.Controllers
{
    [Authorize(Roles = "admin")]
    public class OrderController : Controller
    {
        private readonly ShopHoNoContext _context;

        public OrderController(ShopHoNoContext context)
        {
            _context = context;
        }

        // GET: Order/Index - List all orders
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.OrderItems) // Bao gồm thông tin sản phẩm trong đơn hàng
                .Include(o => o.OrderStatus) // Bao gồm thông tin trạng thái
                .ToList();
            return View(orders);
        }

        // GET: Order/Edit/{id} - Show edit form for an order
        public IActionResult Edit(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderStatus)
                .FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            // Đảm bảo OrderItems không null
            if (order.OrderItems == null)
            {
                order.OrderItems = new List<OrderItem>();
            }

            ViewBag.OrderStatuses = _context.OrderStatuses.ToList();
            ViewBag.Products = _context.Product.ToList();

            return View(order);
        }

        // POST: Order/Edit/{id} - Process order edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Order updatedOrder)
        {
            if (id != updatedOrder.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // Cập nhật thông tin đơn hàng
                var order = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
                if (order == null)
                {
                    return NotFound();
                }

                order.FullName = updatedOrder.FullName;
                order.PhoneNumber = updatedOrder.PhoneNumber;
                order.Address = updatedOrder.Address;
                order.Notes = updatedOrder.Notes;
                order.OrderStatusId = updatedOrder.OrderStatusId;

                // Kiểm tra và cập nhật số lượng sản phẩm
                if (updatedOrder.OrderItems != null)
                {
                    for (int i = 0; i < updatedOrder.OrderItems.Count; i++)
                    {
                        // Kiểm tra xem sản phẩm có tồn tại trong đơn hàng không
                        if (i < order.OrderItems.Count)
                        {
                            order.OrderItems[i].Quantity = updatedOrder.OrderItems[i].Quantity;
                        }
                        else
                        {
                            // Nếu không, có thể thêm sản phẩm mới vào danh sách
                            order.OrderItems.Add(updatedOrder.OrderItems[i]);
                        }
                    }
                }
                else
                {
                    // Nếu OrderItems là null, khởi tạo nó với danh sách rỗng
                    order.OrderItems = new List<OrderItem>();
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Index)); // Redirect to order list
            }
            return View(updatedOrder); // Return the form with errors
        }
        // GET: Order/Delete/{id} - Show delete confirmation
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/{id} - Process order deletion
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index)); // Redirect to the Index action of OrderController
        }

        // GET: Order/Details/{id} - View order details
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
