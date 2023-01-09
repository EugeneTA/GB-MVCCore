using Orders.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_07.Services
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(int buyerId, string address, string phone, IEnumerable<(int productId, int quantity)> products);
    }
}
