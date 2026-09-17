using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using QuanLiKhoHang.Models;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace QuanLiKhoHang.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Signin(string email, string password, bool rememberMe = false)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập email và mật khẩu";
                return RedirectToAction("Signin");
            }

            // Tìm staff theo email với Role và Permissions
            var staff = await _context.Staff
                .Include(s => s.Role)
                .ThenInclude(r => r!.Permissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(s => s.Email == email && s.IsActive);

            if (staff == null)
            {
                TempData["ErrorMessage"] = "Email không tồn tại";
                return RedirectToAction("Signin");
            }

            // Trong thực tế, cần hash password và so sánh
            // Ở đây tạm thời so sánh plain text cho demo
            if (staff.Password != password)
            {
                TempData["ErrorMessage"] = "Mật khẩu không đúng";
                return RedirectToAction("Signin");
            }

            // Tạo claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, staff.StaffId.ToString()),
                new Claim(ClaimTypes.Name, staff.FullName),
                new Claim(ClaimTypes.Email, staff.Email ?? ""),
                new Claim("BranchId", staff.BranchId.ToString()),
                new Claim("RoleId", staff.RoleId.ToString())
            };

            // Thêm các permissions vào claims
            Console.WriteLine($"DEBUG: Staff: {staff.FullName}, Role: {staff.Role?.Name}, RoleId: {staff.RoleId}");
            Console.WriteLine($"DEBUG: Role object: {staff.Role != null}");
            Console.WriteLine($"DEBUG: Permissions collection: {staff.Role?.Permissions != null}");
            Console.WriteLine($"DEBUG: Permissions count: {staff.Role?.Permissions?.Count ?? 0}");

            if (staff.Role?.Permissions != null && staff.Role.Permissions.Count > 0)
            {
                Console.WriteLine($"DEBUG: Found {staff.Role.Permissions.Count} permissions for role {staff.Role.Name}");
                foreach (var rolePermission in staff.Role.Permissions)
                {
                    Console.WriteLine($"DEBUG: RolePermission: {rolePermission.RolePermissionId}, Permission: {rolePermission.Permission?.Name}");
                    if (rolePermission.Permission != null)
                    {
                        Console.WriteLine($"DEBUG: Adding permission: {rolePermission.Permission.Name}");
                        claims.Add(new Claim("Permission", rolePermission.Permission.Name));
                    }
                    else
                    {
                        Console.WriteLine("DEBUG: rolePermission.Permission is null");
                    }
                }
            }
            else
            {
                Console.WriteLine($"DEBUG: No permissions found. Role: {staff.Role?.Name}, Permissions: {staff.Role?.Permissions?.Count ?? 0}");
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Thiết lập thời gian expire dựa trên Remember Me
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = rememberMe
                    ? DateTimeOffset.UtcNow.AddDays(7)  // 7 ngày nếu Remember Me
                    : DateTimeOffset.UtcNow.AddHours(8) // 8 tiếng nếu không
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Phát hành JWT và lưu vào cookie HttpOnly "jwtToken"
            var jwtSection = HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetSection("Jwt");
            var issuer = jwtSection["Issuer"] ?? "WowDash.Auth";
            var audience = jwtSection["Audience"] ?? "WowDash.Client";
            var key = jwtSection["Key"] ?? "change_this_dev_key_to_a_strong_secret_512bits_min";
            var expires = jwtSection["ExpiresMinutes"] != null ? int.Parse(jwtSection["ExpiresMinutes"]!) : 480;

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expires),
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = authProperties.ExpiresUtc?.UtcDateTime ?? DateTime.UtcNow.AddMinutes(expires),
                IsEssential = true,
                Path = "/"
            };
            Response.Cookies.Append("jwtToken", tokenString, cookieOptions);

            TempData["SuccessMessage"] = "Đăng nhập thành công!";
            return RedirectToAction("Index2", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("jwtToken");
            TempData["SuccessMessage"] = "Đăng xuất thành công!";
            return RedirectToAction("Signin");
        }

        public IActionResult Signup()
        {
            return View();
        }
    }
}
