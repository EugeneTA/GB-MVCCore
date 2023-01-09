using Orders.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_06.Services
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(string name, string category, Decimal price);
    }
}
