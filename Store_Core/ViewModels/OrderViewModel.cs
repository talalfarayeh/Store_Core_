using Store.Core.Application.DTOs;

namespace Store_Core.ViewModels
{
    public class OrderViewModel
    {
        public List<OrderDto> Orders { get; set; }
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public required string SearchTerm
        {
            get; set;
        }
    }
}
