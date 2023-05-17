using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Object
{
    [Table("OrderDetail")]
    //[PrimaryKey(nameof(ProductId), nameof(OrderId))]
    public class OrderDetail
    {
        [Key]
        [Column("OrderDetailId")]
        public int OrderDetailId { get; set; }

        [ForeignKey("Product")]
        [Column("ProductId")]       
        public int ProductId { get; set; }
       
        public Product? Product { get; set; }


        [ForeignKey("Order")]
        [Column("OrderId")]        
        public int OrderId { get; set; }
       
        public Order? Order { get; set; }



        [Column("UnitPrice")]
        public decimal UnitPrice { get; set; }

        [Column("Quantity")]
        public int Quantity { get; set; }

    }
}

