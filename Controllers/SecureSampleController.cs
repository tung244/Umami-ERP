using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Ví dụ controller bảo vệ bằng [Authorize] hoạt động với JWT/Cookie.
    /// </summary>
    [Authorize]
    public class SecureSampleController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Trả về thông tin người dùng để xác minh đã xác thực
            return Json(new
            {
                user = User.Identity?.Name,
                authenticated = User.Identity?.IsAuthenticated,
                claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}
