using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Object
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("ProductName")]
        public string ProductName { get; set; }

        [Column("ProductDescription")]
        public string ProductDescription { get; set; }

        [Column("ProductPrice")]
        public decimal ProductPrice { get; set; }

        [Column("UnitInStock")]
        public int UnitInStock { get; set; }


        [ForeignKey("Supplier")]
        [Column("supplierId")]
        public int supplierId { get; set; }

        public Supplier Supplier { get; set; }

        [ForeignKey("ProductCategory")]
        [Column("ProductCategoryId")]
        public int ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public OrderDetail OrderDetails { get; set; }

    }
}
