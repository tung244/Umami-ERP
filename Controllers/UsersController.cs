using Microsoft.AspNetCore.Mvc;

namespace QuanLiKhoHang.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult AddUser()
        {
            return View();
        }
        public IActionResult UsersGrid()
        {
            return View();
        }
        public IActionResult UsersList()
        {
            return View();
        }
        public IActionResult ViewProfile()
        {
            return View();
        }
    }
}
