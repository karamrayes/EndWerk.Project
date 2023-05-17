using Order.Object;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Order.Project.Web.Models
{
    public class OrderModel
    {
       
        public int OrderId { get; set; }

        public decimal OrderAmount { get; set; }

        
        public DateTime OrderDate { get; set; }

        
        public DateTime ShipDate { get; set; }

         
        public bool Shipped { get; set; }

        public bool PaymentRecevied { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public OrderDetail OrderDetails { get; set; }

        

    }
}
