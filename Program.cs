using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QuanLiKhoHang.Middlewares;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Utilities;
using QuanLiKhoHang.Hubs;
using QuanLiKhoHang.Services;

// Thiết lập license EPPlus cho toàn bộ ứng dụng
OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

var builder = WebApplication.CreateBuilder(args);

if (args.Any(arg => string.Equals(arg, "--export-entity-schema", StringComparison.OrdinalIgnoreCase)))
{
    var contentRoot = builder.Environment.ContentRootPath;
    var specFilePath = Path.Combine(contentRoot, "docs", "entity.md");

    if (!File.Exists(specFilePath))
    {
        Console.Error.WriteLine($"Specification file not found at '{specFilePath}'.");
        return;
    }

    var outputPath = Path.Combine(contentRoot, "docs", "entity-schema.json");
    var json = EntitySchemaExporter.ExportToJson(specFilePath);
    File.WriteAllText(outputPath, json);
    Console.WriteLine($"Entity schema exported to '{outputPath}'.");
    return;
}

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Áp dụng filter: trang MVC phải có cookie phiên (redirect nếu mất)
    options.Filters.Add(new QuanLiKhoHang.Filters.RequireSessionCookieForMvcFilter());
});
builder.Services.AddSignalR();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// DataProtection: Lưu key mã hóa cookie bền trong thư mục dpkeys và dùng DPAPI để bảo vệ trên Windows
var keyRingPath = Path.Combine(builder.Environment.ContentRootPath, "dpkeys");
Directory.CreateDirectory(keyRingPath);
if (OperatingSystem.IsWindows())
{
    builder.Services.AddDataProtection()
        .SetApplicationName("WowDash")
        .PersistKeysToFileSystem(new DirectoryInfo(keyRingPath))
        .ProtectKeysWithDpapi();
}
else
{
    builder.Services.AddDataProtection()
        .SetApplicationName("WowDash")
        .PersistKeysToFileSystem(new DirectoryInfo(keyRingPath));
}
// Cấu hình xác thực theo PolicyScheme: nếu có header Authorization -> dùng JWT, ngược lại dùng Cookie
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "SmartScheme";
    options.DefaultAuthenticateScheme = "SmartScheme";
    options.DefaultChallengeScheme = "SmartScheme";
})
.AddPolicyScheme("SmartScheme", "JWT or Cookie", options =>
{
    options.ForwardDefaultSelector = context =>
    {
        var hasBearer = context.Request.Headers.ContainsKey("Authorization");
        return hasBearer ? JwtBearerDefaults.AuthenticationScheme : CookieAuthenticationDefaults.AuthenticationScheme;
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Authentication/Signin";
    options.AccessDeniedPath = "/Authentication/Signin";
    options.Cookie.Name = ".WowDash.Cookies";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.SlidingExpiration = true;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    var jwtSection = builder.Configuration.GetSection("Jwt");
    var issuer = jwtSection["Issuer"] ?? "WowDash.Auth";
    var audience = jwtSection["Audience"] ?? "WowDash.Client";
    var key = jwtSection["Key"] ?? "change_this_dev_key_to_a_strong_secret_512bits_min";
    options.RequireHttpsMetadata = true;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ClockSkew = TimeSpan.FromMinutes(2)
    };
    // Tùy biến Challenge: nếu truy cập trang HTML và token sai/thiếu -> chuyển hướng về Signin thay vì trả về 401
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            // Ngăn hành vi mặc định (trả 401 JSON)
            context.HandleResponse();

            var req = context.HttpContext.Request;
            var accept = req.Headers["Accept"].ToString();
            var isHtml = !string.IsNullOrEmpty(accept) && accept.Contains("text/html", StringComparison.OrdinalIgnoreCase);
            var isApi = req.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);
            var path = req.Path.Value?.ToLowerInvariant() ?? string.Empty;
            var isAuthPath = path.StartsWith("/authentication/signin") || path.StartsWith("/authentication/signout");

            // Không redirect chính trang đăng nhập/đăng xuất để tránh vòng lặp
            if (isAuthPath)
            {
                // Giữ nguyên, không redirect; để MVC render view hoặc controller xử lý
                return Task.CompletedTask;
            }

            if (isHtml && !isApi)
            {
                context.Response.Redirect("/Authentication/Signin");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

// Đăng ký Loyalty Service
builder.Services.AddScoped<LoyaltyService>();

// Đăng ký Report Update Service (SignalR)
builder.Services.AddScoped<IReportUpdateService, ReportUpdateService>();

// Đăng ký Stock Alert Background Service
builder.Services.AddHostedService<StockAlertBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

// Middleware tiêm Authorization header từ cookie JWT (chạy trước UseAuthentication)
app.UseMiddleware<JwtFromCookieMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "publicOrder",
    pattern: "PublicOrder/{action}/{tableId?}",
    defaults: new { controller = "PublicOrder", action = "Index" });

app.MapControllerRoute(
    name: "kitchen",
    pattern: "KitchenTicket/{action}/{id?}",
    defaults: new { controller = "KitchenTicket", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Signin}/{id?}");

app.MapHub<AttendanceHub>("/attendanceHub");
app.MapHub<DishHub>("/dishHub");
app.MapHub<PaymentHub>("/paymentHub");
app.MapHub<ReportHub>("/reportHub");

app.Run();
