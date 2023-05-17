using Order.Object;

namespace Order.Project.Web.Models
{
    public class OrderDetailsModel
    {
        public OrderDetail orderDetails { get; set; }

        public List<OrderDetail> OrderDetailsList { get; set; }

    }
}
