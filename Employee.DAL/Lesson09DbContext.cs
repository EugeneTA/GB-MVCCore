using Lesson09.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lesson09.DAL
{
    public class Lesson09DbContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public Lesson09DbContext(DbContextOptions options) : base(options) { }

    }
}
