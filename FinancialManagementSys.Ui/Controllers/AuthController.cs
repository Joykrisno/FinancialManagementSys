using FinancialManagement.Application.DTOs.Auth;
using FinancialManagement.Web.Models.DTOs;
using FinancialManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LoginDto = FinancialManagement.Web.Models.DTOs.LoginDto;

namespace FinancialManagement.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IApiService apiService, IHttpContextAccessor httpContextAccessor)
        {
            _apiService = apiService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // যদি already logged in থাকে → Redirect
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Home");

            return View(new LoginDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please enter valid credentials.";
                return View(model);
            }

            try
            {
                var request = new LoginRequestDto
                {
                    Email = model.UserName,
                    Password = model.Password
                };

                var response = await _apiService.LoginAsync(request);

                if (response != null && response.Success && response.Data != null)
                {
                    // Token save in session
                    _httpContextAccessor.HttpContext?.Session.SetString("JwtToken", response.Data.Token ?? "");

                    TempData["SuccessMessage"] = "Login successful!";
                    return RedirectToAction("Index", "Home");
                }

                TempData["ErrorMessage"] = response?.Message ?? "Login failed.";
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Login failed: {ex.Message}";
                return View(model);
            }
        }

        public IActionResult Logout()
        {
         
            _httpContextAccessor.HttpContext?.Session.Remove("JwtToken");


            _httpContextAccessor.HttpContext?.Session.Clear();

            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToAction("Login");
        }
    }
}
