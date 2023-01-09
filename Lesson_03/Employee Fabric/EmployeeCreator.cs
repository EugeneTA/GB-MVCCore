using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_03
{
    public class EmployeeCreator : BaseEmployeeCreator
    {
        protected override Employee CreateEmployee(EmployeeType employeeType)
        {
            switch (employeeType)
            {
                case EmployeeType.FullTime: 
                    return new FullTimeEmployee();
                case EmployeeType.PartTime: 
                    return new PartTimeEmployee();
            }

            throw new NotImplementedException();
        }
    }
}
