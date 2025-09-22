using System.Net;

namespace FinancialManagement.Web.Services
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<ApiResponse> PostAsync<T>(string endpoint, T data);
        Task<ApiResponse> PutAsync<T>(string endpoint, T data);
        Task<ApiResponse> DeleteAsync(string endpoint);
    }

    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}