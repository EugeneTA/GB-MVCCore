using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_03
{
    public abstract class Employee
    {
        private string _name = "";
        private string _firstName = "";
        private string _lastName = "";
        private decimal _wageRate = 0;

        // Имя
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        // Фамилия
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }

        // Отчество
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }

        // Ставка оплаты труда
        public decimal WageRate
        {
            get
            {
                return _wageRate;
            }
            set
            {
                _wageRate = value;
            }
        }

        // Метод подсчета средней зароботной платы сотрудника
        public abstract decimal CalculateAverageWage();

        public Employee SetName(string name)
        {
            if (string.IsNullOrEmpty(name) == false)
            {
                Name = name;
            }
            return this;
        }

        public Employee SetFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName) == false)
            {
                FirstName = firstName;
            }
            return this;
        }

        public Employee SetLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName) == false)
            {
                LastName = lastName;
            }
            return this;
        }

        public Employee SetWageRate(decimal wageRate)
        {
            WageRate = wageRate;
            return this;
        }

        public override string ToString()
        {
            return $"Name: {FirstName} {Name} {LastName}\nAverage wage: {CalculateAverageWage()}";
        }

    }
}
