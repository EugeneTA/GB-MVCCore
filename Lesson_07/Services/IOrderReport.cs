using Orders.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_07.Services
{
    public interface IOrderReport
    {
        public Address SellerAddress { get; set; }
        public Buyer Buyer { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long OrderNumber { get; set; }
        public DateTime OrderDateTime { get; set; }
        public IEnumerable<(string name, int count, decimal price)> OrderProducts { get; set; }
        FileInfo Create(string reportTemplateFile);
    }
}
