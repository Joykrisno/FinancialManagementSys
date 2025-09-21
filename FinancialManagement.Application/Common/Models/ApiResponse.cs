namespace FinancialManagement.Application.Common.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public ApiResponse()
        {
        }

        public ApiResponse(T data, string message = "")
        {
            Success = true;
            Data = data;
            Message = message;
        }

        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
        }

        public ApiResponse(List<string> errors)
        {
            Success = false;
            Errors = errors;
        }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();

        public ApiResponse()
        {
        }

        public ApiResponse(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }

        public ApiResponse(List<string> errors)
        {
            Success = false;
            Errors = errors;
        }
    }
}