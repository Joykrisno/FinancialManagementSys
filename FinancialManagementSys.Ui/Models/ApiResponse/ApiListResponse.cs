using System.Collections.Generic;

namespace FinancialManagement.Web.Models.ApiResponse
{
    public class ApiListResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ApiListData<T> Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
