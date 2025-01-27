using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Application.DTOs
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
     
        public string ProductName { get; set; } 
    }
}
