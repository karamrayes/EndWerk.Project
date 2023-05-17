using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Object
{
    [Table("ProductCategory")]
    public class ProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }

        [Column("ProductCategoryDescription")]
        public string ProductCategoryDescription { get; set; }

        [Column("ProductCategoryName")]
        public string ProductCategoryName { get; set; }
    }
}
