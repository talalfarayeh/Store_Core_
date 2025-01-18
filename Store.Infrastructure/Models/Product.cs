namespace Store.Infrastructure.Models
{
    public class Product
    {
        public int ProductID { get; set; }  
        public string ProductName { get; set; }
        public int QuantityInStock { get; set; }
        public decimal UnitPrice { get; set; }

        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }

        public ICollection<StockMovement> StockMovements { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
