
using DemoWebMVC.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebMVC.Controllers
{
    public class UserController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult CheckLogin(LoginDTO dto)
        {
            return Ok(dto);
        }

        public IActionResult Register()
        {
            return View();
        }

    }
}
