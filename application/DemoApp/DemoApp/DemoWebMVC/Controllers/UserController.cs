using DemoWebMVC.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DemoWebMVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckLogin([FromBody] LoginDTO dto)
        {
            return Ok(dto);
        }

        public IActionResult Register()
        {
            return View();
        }

    }
}
