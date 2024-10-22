using Microsoft.AspNetCore.Mvc;
using ShopHoNo.Data;
using ShopHoNo.Models;
using ShopHoNo.Controllers.Session;
using ShopHoNo.Models.ViewModels;
using ShopHoNo.Controllers.KEY;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ShopHoNo.Controllers
{
    public class CartController : Controller
	{
		private readonly ShopHoNoContext _context;

		public CartController(ShopHoNoContext context)
		{
			_context = context;
		}

		public static string CART_KEY = "MYCART";
		public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

		public IActionResult Index()
		{
			return View(Cart);
		}

		public IActionResult AddToCart(int id, int quantity = 1)
		{
			var cart = Cart;
			var item = cart.SingleOrDefault(x => x.Id == id);

			if (item == null)
			{
				var product = _context.Product.SingleOrDefault(x => x.Id == id);
				if (product == null)
				{
					TempData["Message"] = $"No product found with ID {id}";
					return Redirect("/404");
				}

				item = new CartItem
				{
					Id = product.Id,
					Name = product.Name,
					Price = product.Price,
					image = product.Image,
					quantity = quantity
				};
				cart.Add(item);
			}
			else
			{
				item.quantity += quantity; // Update quantity
			}

			HttpContext.Session.Set(MySetting.CART_KEY, cart);
			return RedirectToAction("Index");
		}

		public IActionResult RemoveCart(int id)
		{
			var cart = Cart;
			var item = cart.SingleOrDefault(p => p.Id == id); // Find the item by its ID

			if (item != null) // Check if the item exists
			{
				cart.Remove(item); // Remove the item from the cart
				HttpContext.Session.Set(MySetting.CART_KEY, cart); // Update the session
			}

			return RedirectToAction("Index"); // Redirect back to the cart index
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Lấy ID người dùng từ Identity
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    ModelState.AddModelError("", "Invalid user ID. Please log in again.");
                    return RedirectToAction("Index", "Account"); // Chuyển hướng về trang login 
                }

                var order = new Order
                {
                    FullName = model.Name,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Notes = model.Notes,
                    DateTime = DateTime.Now,
                    OrderStatusId = 1, // Giả sử 1 là ID cho trạng thái "Đang chờ"
                    OrderItems = new List<OrderItem>(),
                    UserId = userId // Gán UserId từ Identity
                };

                // Lấy các mặt hàng từ giỏ hàng
                var cartItems = Cart;
                foreach (var item in cartItems)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.Id,
                        ProductName = item.Name,
                        Price = item.Price,
                        Quantity = item.quantity
                    });
                }

                // Thêm đơn hàng vào cơ sở dữ liệu
                _context.Orders.Add(order);
                _context.SaveChanges();

                // Xóa giỏ hàng sau khi đặt hàng
                HttpContext.Session.Remove(MySetting.CART_KEY);

                // Thêm thông báo thành công
                TempData["SuccessMessage"] = "Your order has been placed successfully!";
                return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chính
            }

            return RedirectToAction("Index", "Account"); // Chuyển hướng về trang login 
        }

    }
}