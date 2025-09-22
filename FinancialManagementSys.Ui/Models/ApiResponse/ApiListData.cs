using System.Collections.Generic;

namespace FinancialManagement.Web.Models.ApiResponse
{
    public class ApiListData<T>
    {
        public List<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}
