using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace FinancialManagement.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<BaseController> _logger;

        public BaseController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<BaseController> logger = null)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        protected HttpClient GetHttpClient()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            
            _logger?.LogInformation($"API Base Address: {client.BaseAddress}");
            _logger?.LogInformation($"Timeout: {client.Timeout}");

            
            var token = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                _logger?.LogInformation("JWT Token added to request");
            }

            return client;
        }

        protected async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                using var client = GetHttpClient();
                var fullUrl = new Uri(client.BaseAddress, endpoint);

                _logger?.LogInformation($"Making GET request to: {fullUrl}");

                var response = await client.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger?.LogInformation($"GET Response Status: {response.StatusCode}");
                _logger?.LogInformation($"GET Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }

                _logger?.LogWarning($"GET request failed: {response.StatusCode} - {responseContent}");
                return default;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error in GetAsync for endpoint: {endpoint}");
                throw;
            }
        }

        protected async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                using var client = GetHttpClient();
                var fullUrl = new Uri(client.BaseAddress, endpoint);

                _logger?.LogInformation($"Making POST request to: {fullUrl}");

                var json = JsonConvert.SerializeObject(data);
                _logger?.LogInformation($"POST Request payload: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger?.LogInformation($"POST Response Status: {response.StatusCode}");
                _logger?.LogInformation($"POST Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }

                
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        _logger?.LogWarning("API returned 401 Unauthorized");
                        break;
                    case HttpStatusCode.BadRequest:
                        _logger?.LogWarning($"API returned 400 Bad Request: {responseContent}");
                        break;
                    case HttpStatusCode.NotFound:
                        _logger?.LogWarning($"API endpoint not found: {fullUrl}");
                        break;
                    case HttpStatusCode.InternalServerError:
                        _logger?.LogError($"API internal server error: {responseContent}");
                        break;
                    default:
                        _logger?.LogWarning($"API returned {response.StatusCode}: {responseContent}");
                        break;
                }

                return default;
            }
            catch (HttpRequestException httpEx)
            {
                _logger?.LogError(httpEx, $"HTTP request exception for endpoint: {endpoint}");
                _logger?.LogError($"HttpRequestException Message: {httpEx.Message}");
                throw new Exception($"Failed to connect to API. Please ensure API is running at the configured URL.", httpEx);
            }
            catch (TaskCanceledException tcEx) when (tcEx.InnerException is TimeoutException)
            {
                _logger?.LogError(tcEx, $"Request timeout for endpoint: {endpoint}");
                throw new Exception("Request timeout. API took too long to respond.", tcEx);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Unexpected error in PostAsync for endpoint: {endpoint}");
                throw;
            }
        }

        protected async Task<T?> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                using var client = GetHttpClient();
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger?.LogInformation($"PUT Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }

                return default;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error in PutAsync for endpoint: {endpoint}");
                throw;
            }
        }

        protected async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                using var client = GetHttpClient();
                var response = await client.DeleteAsync(endpoint);

                _logger?.LogInformation($"DELETE Response Status: {response.StatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error in DeleteAsync for endpoint: {endpoint}");
                return false;
            }
        }

        protected void SetSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        protected void SetErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
        }

        
        protected async Task<bool> TestApiConnection()
        {
            try
            {
                using var client = GetHttpClient();
                var response = await client.GetAsync("health");
                _logger?.LogInformation($"API Health Check Status: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "API connection test failed");
                return false;
            }
        }
    }
}