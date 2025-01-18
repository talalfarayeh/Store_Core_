using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Application.DTOs
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int QuantityInStock { get; set; }
        public decimal UnitPrice { get; set; }

        public string SupplierName { get; set; }
    }
}
