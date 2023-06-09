using Order.Object;

namespace EndWerk.API2.Dto
{
    public class OrderDTO
    {
        //public int ProductId { get; set; }

        public List<ProductOrderDTO> Products { get; set; }

        //public int Quantity { get; set; }

        public DateTime ShipDate { get; set; }

        public DateTime OrderDate { get; set; }

        public bool Shipped { get; set; }

        public bool PaymentRecevied { get; set; }

        public string UserId { get; set; }

        //public OrderDetail orderDetail { get; set; }

        //public Order.Object.Order Order { get; set; }

        //public decimal UnitPrice { get; set; }

        //public ProductDTO ProductOrderDto { get; set; }






    }
}
