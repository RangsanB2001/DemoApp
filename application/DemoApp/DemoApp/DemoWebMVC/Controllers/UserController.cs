using AuthenticationModels;
using Helpers.CallHttpClientHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Settings;
using System.Data;
using System.Text.Json;

namespace DemoWebMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ServiceSettings serviceSettings;
        private readonly ICallHttpClientHelper httpClientHelper;

        public UserController(IOptions<ServiceSettings> serviceSettings, ICallHttpClientHelper httpClientHelper)
        {
            this.serviceSettings = serviceSettings.Value;
            this.httpClientHelper = httpClientHelper;
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> CheckLoginAsync(LoginRequest request)
        {
            var fullUrl = $"{serviceSettings.AuthenticationApi}/User/Login";
            var requestContent = JsonSerializer.Serialize(request);

            var result = await httpClientHelper.SendJson(fullUrl, HttpMethod.Post, requestContent);

            if (result.success)
            {
                var userModel = JsonSerializer.Deserialize<LoginResponse>(result.body);
                HttpContext.Session.SetInt32("user_id", userModel.iduser);
                return RedirectToAction("Profile", "User");
            }
            else
            {
                return Unauthorized();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> RegisterUser(AddUserRequest AddUser)
        {
            var fullUrl = $"{serviceSettings.AuthenticationApi}/User/Register";
            var requestContent = JsonSerializer.Serialize(AddUser);

            var result = await httpClientHelper.SendJson(fullUrl, HttpMethod.Post, requestContent);

            if (result.success)
            {

                return View();
            }
            else
            {
                return Unauthorized();
            }

        }

        public async Task<IActionResult> ProfileAsync()
        {
            var userId = HttpContext.Session.GetInt32("user_id");

            if (userId != null)
            {
                var fullUrl = $"{serviceSettings.AuthenticationApi}/User/Profile/?iduser={userId.Value}";
                var result = await httpClientHelper.SendJson(fullUrl, HttpMethod.Get, string.Empty);

                if (result.success)
                {
                    var userProfile = JsonSerializer.Deserialize<GetDataUser>(result.body);

                    if (userProfile != null)
                    {
                        return View(userProfile);
                    }
                }
            }


            return RedirectToAction("Login", "User");
        }

        public async Task<IActionResult> UpdateProfileUserAsync(GetDataUser Update)
        {
            var fullUrl = $"{serviceSettings.AuthenticationApi}/User/UpdateUser";
            var requestContent = JsonSerializer.Serialize(Update);
            var result = await httpClientHelper.SendJson(fullUrl, HttpMethod.Patch, requestContent);

            if (result.success)
            {
                var userModel = JsonSerializer.Deserialize<GetDataUser>(result.body);
                return View("Profile", userModel);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
