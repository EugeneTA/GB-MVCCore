using Orders.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_07.Models.Reports
{
    public class OrderInfo
    {
        public Seller Seller { get; set; }

        public long OrderNumber { get; set; }
        public DateTime OrderDateTime { get; set; }

        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public Buyer Buyer { get; set; } = null!;

        public IEnumerable<OrderItem> Items { get; set; }
    }
}
