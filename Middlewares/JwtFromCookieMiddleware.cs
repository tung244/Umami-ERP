using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace QuanLiKhoHang.Middlewares
{
    /// <summary>
    /// Middleware tiêm header Authorization từ cookie HttpOnly "jwtToken" vào request để JWT Bearer auth xử lý.
    /// </summary>
    public class JwtFromCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtFromCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Tránh tiêm trên các endpoint đăng nhập/đăng xuất để không tạo vòng lặp redirect
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;
            var isAuthPath = path.StartsWith("/authentication/signin") || path.StartsWith("/authentication/signout");

            // Luôn tiêm Authorization từ cookie jwtToken nếu header chưa có (mọi request), trừ trang đăng nhập/đăng xuất
            if (!isAuthPath && !context.Request.Headers.ContainsKey("Authorization"))
            {
                if (context.Request.Cookies.TryGetValue("jwtToken", out var token) && !string.IsNullOrWhiteSpace(token))
                {
                    context.Request.Headers["Authorization"] = $"Bearer {token}";
                }
            }

            await _next(context);
        }
    }
}
