using FinancialManagement.Application.DTOs.JournalEntry;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FinancialManagementSystem.Web.Controllers
{
    public class JournalEntryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public JournalEntryController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["ApiBaseUrl"]); 
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
                var response = await client.GetAsync("api/JournalEntry");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var journalEntries = JsonSerializer.Deserialize<List<JournalEntryDto>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return Json(journalEntries);
                }
                else
                {
                    return Json(new List<JournalEntryDto>());
                }
            }
            catch (Exception ex)
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
        public async Task<IActionResult> Create(JournalEntryDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/JournalEntry", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to create Journal Entry");
            return View(model);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/JournalEntry/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var json = await response.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<JournalEntryDto>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(model);
        }

   
        [HttpPost]
        public async Task<IActionResult> Edit(JournalEntryDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/JournalEntry/{model.Id}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to update Journal Entry");
            return View(model);
        }
    }
}