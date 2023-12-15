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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Login", "User");
            }
            else
            {
                return RedirectToAction("Register", "User");
            }

        }

        public async Task<IActionResult> Profile()
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

                if (userModel != null)
                {
                    HttpContext.Session.SetInt32("user_id", userModel.iduser);
                    return RedirectToAction("Profile", "User");
                }
            }

            HttpContext.Session.SetInt32("user_id", Update.iduser);
            return RedirectToAction("Profile", "User");
        }


        public async Task<IActionResult> Data()
        {
            var userId = HttpContext.Session.GetInt32("user_id");

            if (userId != null)
            {
                var fullUrl = $"{serviceSettings.AuthenticationApi}/User/GetData/?iduser={userId.Value}";
                var result = await httpClientHelper.SendJson(fullUrl, HttpMethod.Get, string.Empty);

                if (result.success)
                {
                    var data = JsonSerializer.Deserialize<IEnumerable<GetDataUser>>(result.body);

                    if (data != null)
                    {
                        return View(data);
                    }
                }
            }
            return RedirectToAction("Login", "User");
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id.HasValue)
            {
                var fullUrl = $"{serviceSettings.AuthenticationApi}/User/DeleteUser/?iduser={id.Value}";
                var result = await httpClientHelper.SendJson(fullUrl, HttpMethod.Delete, string.Empty);

                if (result.success)
                {
                    return RedirectToAction("Data", "User");
                }
            }
            return RedirectToAction("Data", "User");
        }
    }
}
