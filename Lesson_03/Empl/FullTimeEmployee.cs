using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_03
{
    internal class FullTimeEmployee : Employee
    {

        public override decimal CalculateAverageWage()
        {
            return WageRate;
        }

        public override string ToString()
        {
            return $"{base.ToString()}\nEmployee type: Full time";
        }
    }
}
