namespace Store.Infrastructure.Models
{
    public class StockMovement
    {
        public int StockMovementID { get; set; }  
        public string MovementType { get; set; }  
        public int Quantity { get; set; }
        public DateTime MovementDate { get; set; }
 
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }

}
