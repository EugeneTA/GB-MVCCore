using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_03
{
    public class PartTimeEmployee : Employee
    {
        public override decimal CalculateAverageWage()
        {
            return (decimal)20.8 * 8 * WageRate;
        }

        public override string ToString()
        {
            return $"{base.ToString()}\nEmployee type: Part time";
        }
    }
}
