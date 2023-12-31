﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Order.Object
{
    [Table("OrderDetail")]
    //[PrimaryKey(nameof(ProductId), nameof(OrderId))] another type of key
    public class OrderDetail
    {
        [Key]
        [Column("OrderDetailId")]
        public int OrderDetailId { get; set; }

        
        [Column("ProductId")]       
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }


        [ForeignKey("Order")]
        [Column("OrderId")]        
        public int OrderId { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }



        [Column("UnitPrice")]
        public decimal UnitPrice { get; set; }

        [Column("Quantity")]
        public int Quantity { get; set; }

    }
}

