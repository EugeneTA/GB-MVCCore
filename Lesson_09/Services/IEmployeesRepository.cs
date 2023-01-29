using Lesson09.DAL.Entities;

namespace Lesson09.Services
{
    public interface IEmployeesRepository
    {
        IEnumerable<Employee> GetAll();

        Employee? GetById(int Id);

        int Add(Employee item);

        bool Edit(Employee item);

        bool Remove(int Id);
    }
}
