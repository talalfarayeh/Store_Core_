using System.ComponentModel.DataAnnotations;

namespace Store.Infrastructure.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }
       
        public string SupplierName { get; set; }
        
        public string Phone { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
