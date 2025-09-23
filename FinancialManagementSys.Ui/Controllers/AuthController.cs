using FinancialManagement.Application.DTOs.Auth;
using FinancialManagement.Web.Models.DTOs;
using FinancialManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
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

            var request = new LoginRequestDto
            {
                Email = model.UserName,   
                Password = model.Password
            };

            var response = await _apiService.LoginAsync(request);

            if (response != null && response.Success && response.Data != null)
            {
                _httpContextAccessor.HttpContext?.Session.SetString("JwtToken", response.Data.Token ?? "");

                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = response?.Message ?? "Login failed.";
            return View(model);
        }

        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("JwtToken");
            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToAction("Login");
        }
    }
}
