using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ShopHoNo.Data;
using ShopHoNo.Models;
using System.Configuration;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

// Cấu hình DbContext
builder.Services.AddDbContext<ShopHoNoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopHoNoContext") ?? throw new InvalidOperationException("Connection string 'ShopHoNoContext' not found.")));

// Identity
builder.Services.AddIdentity<AppUserModel, IdentityRole>()
    .AddEntityFrameworkStores<ShopHoNoContext>()
    .AddDefaultTokenProviders();

// Cấu hình IdentityOptions
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;

    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

services.AddAuthentication()
.AddGoogle(googleOptions =>
{
    var googleClientId = configuration.GetSection("Authentication:Google:ClientId").Value;
    var googleClientSecret = configuration.GetSection("Authentication:Google:ClientSecret").Value;

    if (googleClientId != null && googleClientSecret != null)
    {
        googleOptions.ClientId = googleClientId;
        googleOptions.ClientSecret = googleClientSecret;
        googleOptions.Scope.Add("email"); // Ensure email is requested
        googleOptions.SaveTokens = true;
    }
})
.AddFacebook(facebookOptions =>
{
    var facebookAppId = configuration.GetSection("Authentication:Facebook:AppId").Value;
    var facebookAppSecret = configuration.GetSection("Authentication:Facebook:AppSecret").Value;

    if (facebookAppId != null && facebookAppSecret != null)
    {
        facebookOptions.AppId = facebookAppId;
        facebookOptions.AppSecret = facebookAppSecret;
        facebookOptions.Scope.Add("email"); // Đảm bảo yêu cầu quyền truy cập email
        facebookOptions.SaveTokens = true;
    }
});





// Dịch vụ gửi email
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddControllersWithViews();

// Cấu hình Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Chính sách Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

// Dịch vụ MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Sử dụng Session
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
// Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
