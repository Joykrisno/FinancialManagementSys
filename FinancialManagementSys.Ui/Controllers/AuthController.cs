using Microsoft.AspNetCore.Mvc;
using FinancialManagement.Application.DTOs.Auth;
using FinancialManagement.Application.Common.Models;

namespace FinancialManagement.Web.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
            : base(httpClientFactory, configuration)
        {
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, redirect to dashboard
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken")))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await PostAsync<ApiResponse<LoginResponseDto>>("auth/login", model);

                if (result?.Success == true && result.Data != null)
                {
                    // Store JWT token in session
                    HttpContext.Session.SetString("JwtToken", result.Data.Token);
                    HttpContext.Session.SetString("UserName", result.Data.UserName);
                    HttpContext.Session.SetString("UserEmail", result.Data.Email);
                    HttpContext.Session.SetString("UserRole", result.Data.Role);

                    SetSuccessMessage("Login successful!");
                    return RedirectToAction("Index", "Dashboard");
                }

                ModelState.AddModelError("", result?.Message ?? "Login failed");
                SetErrorMessage(result?.Message ?? "Login failed");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login");
                SetErrorMessage("An error occurred during login: " + ex.Message);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            SetSuccessMessage("Logged out successfully!");
            return RedirectToAction("Login");
        }
    }
}