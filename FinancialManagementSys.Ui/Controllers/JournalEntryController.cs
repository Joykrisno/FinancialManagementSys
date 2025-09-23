using FinancialManagement.Application.DTOs.JournalEntry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FinancialManagement.Web.Controllers
{
    public class JournalEntryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public JournalEntryController(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7162/api/");
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJournalEntries()
        {
            try
            {
                var client = CreateClient();
                var response = await client.GetAsync("JournalEntry"); // Api endpoint

                if (!response.IsSuccessStatusCode)
                    return Json(new List<JournalEntryDto>());

                var json = await response.Content.ReadAsStringAsync();
                var journalEntries = JsonSerializer.Deserialize<List<JournalEntryDto>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Json(journalEntries ?? new List<JournalEntryDto>());
            }
            catch
            {
                return Json(new List<JournalEntryDto>());
            }
        }

        public IActionResult Create()
        {
            var model = new JournalEntryDto
            {
                Lines = new List<JournalEntryLineDto> { new JournalEntryLineDto() }
            };
            return View(model);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create(JournalEntryDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var client = CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("JournalEntry", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Journal Entry created successfully!";
                    TempData["AlertType"] = "success"; 
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["AlertMessage"] = "Failed to create Journal Entry!";
                    TempData["AlertType"] = "error";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = $"Error: {ex.Message}";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Index));
            }
        }



        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"JournalEntry/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load journal entry.";
                return RedirectToAction(nameof(Index));
            }

            var json = await response.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<JournalEntryDto>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(model ?? new JournalEntryDto());
        }


        [HttpPost]  
        public async Task<IActionResult> Edit(JournalEntryDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"JournalEntry/{model.Id}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to update Journal Entry");
            return View(model);
        }
    }
}
