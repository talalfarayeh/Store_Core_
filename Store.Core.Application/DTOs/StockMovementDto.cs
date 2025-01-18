using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Application.DTOs
{
    public class StockMovementDto
    {
        public int StockMovementID { get; set; }
        public string MovementType { get; set; }
        public int Quantity { get; set; }
        public DateTime MovementDate { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
}
