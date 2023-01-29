using Lesson09.DAL;
using Lesson09.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lesson09.Services.Impl
{
    public class EmployeesRepository : IEmployeesRepository
    {

        private readonly Lesson09DbContext _dbContext;

        public EmployeesRepository(Lesson09DbContext dbContext)
        {
            _dbContext = dbContext;
            dbContext.Database.EnsureCreated();
        }

        public IEnumerable<Employee> GetAll() => _dbContext.Employees.ToList();

        public int Add(Employee data)
        {
            if (data == null) return -1;
            _dbContext.Employees.Add(data);
            _dbContext.SaveChanges();
            return data.Id;
        }

        public bool Edit(Employee data)
        {
            if (data != null)
            {
                Employee employee = GetById(data.Id);

                if (employee != null)
                {
                    employee.Name = employee.Name == data.Name ? employee.Name : data.Name;
                    employee.LastName = employee.LastName == data.LastName ? employee.LastName : data.LastName;
                    employee.Patronymic = employee.Patronymic == data.Patronymic ? employee.Patronymic : data.Patronymic;
                    employee.Birthday = employee.Birthday == data.Birthday ? employee.Birthday : data.Birthday;

                    return _dbContext.SaveChanges() > 0;
                }
            }

            return false;
        }

        public Employee? GetById(int Id) => _dbContext.Employees.FirstOrDefault(empl => empl.Id == Id);

        public bool Remove(int Id)
        {
            Employee employee = GetById(Id);
            if (employee == null) return false;
            _dbContext.Employees.Remove(employee);
            return _dbContext.SaveChanges() > 0;
        }
    }
}
