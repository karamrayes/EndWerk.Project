using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Object
{
    [Table("Message")]
    public class Message
    {
        [Key]
        [Column("MessageId")]
        public int MessageId { get; set; }

        [Column("Content")]
        public string Content { get; set; }

        [Column("Type")]
        public string Type { get; set; }

        [Column("UserId")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
