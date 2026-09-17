using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuanLiKhoHang.Filters
{
    /// <summary>
    /// Buộc trang MVC phải có cookie phiên (Cookie Auth). Nếu thiếu -> chuyển hướng về trang đăng nhập.
    /// Bỏ qua với API (đường dẫn bắt đầu /api) và các action có [AllowAnonymous].
    /// </summary>
    public class RequireSessionCookieForMvcFilter : IAsyncActionFilter
    {
        private const string SessionCookieName = ".WowDash.Cookies";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var req = httpContext.Request;

            // Bỏ qua API
            if (req.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
            {
                await next();
                return;
            }

            // Cho phép các endpoint đăng nhập/đăng xuất đi qua filter
            var path = req.Path.Value?.ToLowerInvariant() ?? string.Empty;

            // Whitelist theo controller/action để đảm bảo các endpoint nội bộ (AJAX) cũng hoạt động
            var controller = (context.RouteData.Values["controller"]?.ToString() ?? string.Empty).ToLowerInvariant();
            var action = (context.RouteData.Values["action"]?.ToString() ?? string.Empty).ToLowerInvariant();

            var isAuthEndpoint = controller == "authentication" && (action == "signin" || action == "signout");
            var isPublicOrder = controller == "publicorder"; // Mở toàn bộ PublicOrder (Table, Checkout, AddToCart,...)
            var isKitchenTicket = controller == "kitchenticket"; // Mở toàn bộ KitchenTicket (Index, GetTickets, UpdateItemStatus,...)

            if (isAuthEndpoint || isPublicOrder || isKitchenTicket)
            {
                await next();
                return;
            }

            // Bỏ qua AllowAnonymous
            var endpoint = httpContext.GetEndpoint();
            var allowAnon = endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() != null;
            if (allowAnon)
            {
                await next();
                return;
            }

            // Nếu thiếu cookie phiên -> redirect Signin
            if (!req.Cookies.ContainsKey(SessionCookieName))
            {
                context.Result = new RedirectToActionResult("Signin", "Authentication", null);
                return;
            }

            await next();
        }
    }
}
