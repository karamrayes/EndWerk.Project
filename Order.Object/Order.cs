using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Order.Object
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [Column("OrderId")]
        public int OrderId { get; set; }

        [Column("OrderAmount")]
        public decimal OrderAmount { get; set; }

        [Column("OrderDate")]
        public DateTime OrderDate { get; set; }

        [Column("ShipDate")]
        public DateTime ShipDate { get; set; }

        [Column("Shipped")]
        public bool Shipped { get; set; }

        [Column("PaymentRecevied")]
        public bool PaymentRecevied { get; set; }

        [Column("UserId")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

        public OrderDetail OrderDetail { get; set; } //help table

        //public Product Product { get; set; }

    }
}