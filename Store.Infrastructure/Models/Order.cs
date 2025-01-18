namespace Store.Infrastructure.Models
{
    public class Order
    {
        public int OrderID { get; set; }  
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }

       
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }

}
