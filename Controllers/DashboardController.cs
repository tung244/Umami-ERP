using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace QuanLiKhoHang.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index2()
        {
            // ĐẨY JWT RA TOAST & LOG (chỉ phục vụ debug UI theo yêu cầu)
            // Lưu ý: Việc lộ JWT ra UI là rủi ro bảo mật, chỉ nên dùng trong môi trường dev
            var token = Request.Cookies["jwtToken"];
            if (!string.IsNullOrEmpty(token))
            {
                // Dùng cho console.log ở View
                ViewBag.JwtForDebug = token; // string
                // Bắn toast qua TempData sử dụng partial _Toast
                TempData["InfoMessage"] = $"JWT: {token}";
            }
            else
            {
                ViewBag.JwtForDebug = string.Empty;
            }
            return View();
        }
        public IActionResult Index3()
        {
            return View();
        }
        public IActionResult Index4()
        {
            return View();
        }
        public IActionResult Index5()
        {
            return View();
        }
        public IActionResult Index6()
        {
            return View();
        }
        public IActionResult Index7()
        {
            return View();
        }
        public IActionResult Index8()
        {
            return View();
        }
        public IActionResult Index9()
        {
            return View();
        }
        public IActionResult Index10()
        {
            return View();
        }
    }
}
