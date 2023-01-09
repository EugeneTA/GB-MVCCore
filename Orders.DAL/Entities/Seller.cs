using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.DAL.Entities
{
    [Table("Sellers")]
    public class Seller: NamedEntity
    {
        public Address? SellerAddress { get; set; }
    }
}
