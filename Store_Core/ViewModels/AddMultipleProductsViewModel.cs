using Store.Core.Application.DTOs;

namespace Store_Core.ViewModels
{
    public class AddMultipleProductsViewModel
    {
        public List<ProductCreateUpdateDto> Products { get; set; } = new List<ProductCreateUpdateDto>();
    }
}
