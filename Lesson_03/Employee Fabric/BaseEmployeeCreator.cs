using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_03
{
    public abstract class BaseEmployeeCreator
    {
        /// <summary>
        /// Фабричный метод создания нового работника
        /// </summary>
        /// <param name="employeeType">Тип создаваемого работника</param>
        /// <returns></returns>
        public Employee NewEmployee(EmployeeType employeeType)
        {
            return CreateEmployee(employeeType);
        }

        /// <summary>
        /// Фабричный метод создания нового работника с заполнением случайными данными
        /// </summary>
        /// <param name="employeeType">Тип создаваемого работника</param>
        /// <param name="namesDB">База имен, фамилий, отчеств</param>
        /// <param name="maxWageRate">Максимальная ставка</param>
        /// <returns></returns>
        public Employee NewEmployee(EmployeeType employeeType, NamesDB namesDB, int maxWageRate)
        {
            Employee employee = CreateEmployee(employeeType);
            Random random = new Random();

            if (namesDB != null)
            {

                switch (random.Next(0, 2))
                {
                    case 0:
                        {
                            employee?
                                .SetName(namesDB.MNames[random.Next(0, namesDB.MNames.Count)])
                                .SetFirstName(namesDB.MFirstNames[random.Next(0, namesDB.MFirstNames.Count)])
                                .SetLastName(namesDB.MLastNames[random.Next(0, namesDB.MLastNames.Count)])
                                .SetWageRate(random.Next(1, maxWageRate));
                            break;
                        }
                    default:
                        {
                            employee?
                                .SetName(namesDB.FNames[random.Next(0, namesDB.FNames.Count)])
                                .SetFirstName(namesDB.FFirstNames[random.Next(0, namesDB.FFirstNames.Count)])
                                .SetLastName(namesDB.FLastNames[random.Next(0, namesDB.FLastNames.Count)])
                                .SetWageRate(random.Next(1, maxWageRate));
                            break;
                        }
                }

            }

            return employee;
        }

        protected abstract Employee CreateEmployee(EmployeeType employeeType);
    }
}
