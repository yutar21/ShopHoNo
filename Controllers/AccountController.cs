using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ShopHoNo.Models;
using ShopHoNo.Models.ViewModels;
using System.Security.Claims;

namespace ShopHoNo.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signInManager;
        private IEmailSender _emailSender;

        public AccountController(SignInManager<AppUserModel> signInManager,
                                 UserManager<AppUserModel> userManager,
                                 IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register(string email = null) // Thêm tham số email
        {
            var model = new User();
            if (!string.IsNullOrEmpty(email))
            {
                model.Email = email; // Gán email từ tham số
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Cart");
                }
                ModelState.AddModelError("", "Invalid username and password");
            }
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email };
                var result = await _userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    TempData["success"] = "Register success!";
                    return Redirect("/account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View("Register", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string url = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(url);
        }

        // Forgot Password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

            await _emailSender.SendEmailAsync(model.Email, "Reset Password", $"Click <a href='{resetLink}'>here</a> to reset your password.");

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        // Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Index", "Account");
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (result?.Succeeded == true)
            {
                var claims = result.Principal.Claims;
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                //var user = await _userManager.FindByEmailAsync(email);
            /*   if (user == null)
                {
                    user = new AppUserModel { UserName = email, Email = email };
                    await _userManager.CreateAsync(user);
                }*/
                // Lưu thông báo vào TempData
                TempData["Message"] = "You have successfully authenticated with Google, please register an account.";
                // Chuyển hướng đến trang đăng ký và truyền email
                return RedirectToAction("Register", "Account", new { email = email });
            }

            return RedirectToAction("Index", "Account");
        }


        [HttpGet]
        public IActionResult LoginWithFacebook()
        {
            var redirectUrl = Url.Action("FacebookResponse", "Account", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            if (result?.Succeeded == true)
            {
                var claims = result.Principal.Claims;
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                var user = await _userManager.FindByEmailAsync(email);
            /*  if (user == null)
                {
                    user = new AppUserModel { UserName = email, Email = email };
                    await _userManager.CreateAsync(user);
                }*/

                // Chuyển hướng đến trang đăng ký và truyền email
                return RedirectToAction("Register", "Account", new { email = email });
            }

            return RedirectToAction("Index", "Account");
        }


    }
}
