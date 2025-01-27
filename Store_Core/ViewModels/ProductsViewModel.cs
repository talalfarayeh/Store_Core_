using Store.Core.Application.DTOs;
using Store_Core.ViewModels;

public class ProductsViewModel
{
    public List<ProductDto> Products { get; set; }
    public int TotalRecords { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public string SearchTerm { get; set; }
}