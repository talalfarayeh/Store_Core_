using System.ComponentModel.DataAnnotations;

namespace Store.Infrastructure.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        [Required(ErrorMessage = "Supplier Name is required")]
        public string SupplierName { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
