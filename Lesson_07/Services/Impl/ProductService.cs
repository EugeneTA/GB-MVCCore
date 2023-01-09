using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Orders.DAL;
using Orders.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_07.Services.Impl
{
    public class ProductService : IProductService
    {
        #region Services

        private readonly ILogger<OrderService> _logger;
        private readonly OrdersDbContext _context;

        #endregion

        #region Constructors

        public ProductService(OrdersDbContext context,
            ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion
        public async Task<Product> AddProductAsync(string name, string category, decimal price)
        {
            if (_context == null)
            {
                throw new ArgumentNullException("Database context not initialized");
            }

            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Product name is empty");
            }

            if (String.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException("Product category name is empty");
            }

            var product = new Product
            {
                Name = name,
                Category = category,
                Price = price,
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
