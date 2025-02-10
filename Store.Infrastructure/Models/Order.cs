using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Store.Infrastructure.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }  
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }

       
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }

}
