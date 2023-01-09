using Orders.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_07.Models.Reports
{
    public class ProductsCatalog
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
