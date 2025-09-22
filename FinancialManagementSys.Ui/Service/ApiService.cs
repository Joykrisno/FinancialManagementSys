using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FinancialManagement.Web.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;
        }

        private Task AddJwtTokenAsync()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            return Task.CompletedTask;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            await AddJwtTokenAsync();

            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return default;
        }

        public async Task<ApiResponse> PostAsync<T>(string endpoint, T data)
        {
            await AddJwtTokenAsync();

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);

            return new ApiResponse
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = response.StatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync()
            };
        }

        public async Task<ApiResponse> PutAsync<T>(string endpoint, T data)
        {
            await AddJwtTokenAsync();

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, content);

            return new ApiResponse
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = response.StatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync()
            };
        }

        public async Task<ApiResponse> DeleteAsync(string endpoint)
        {
            await AddJwtTokenAsync();

            var response = await _httpClient.DeleteAsync(endpoint);

            return new ApiResponse
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = response.StatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync()
            };
        }
    }
}
