using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Object
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        [Column("SupplierId")]
        public int SupplierId { get; set; }

        
        [Column("SupplierName")]
        public string SupplierName { get; set; }

        
        [Column("SupplierAdress")]
        public string SupplierAdress { get; set; }

        
        [Column("SupplierEmail")]
        public string SupplierEmail { get; set; }
    }
}
