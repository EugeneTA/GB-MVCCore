using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.DAL.Entities
{
    [Table("OrderItems")]
    public class OrderItem : Entity
    {
        public Product ProdItem { get; set; }

        public int Quantity { get; set; }

        [Required]
        public Order? Order { get; set; }
    }
}
