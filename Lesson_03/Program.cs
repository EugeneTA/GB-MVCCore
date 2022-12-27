
namespace Lesson_03
{
    #region Задание

    /*
    
       1. Построить три класса (базовый и 2 потомка), описывающих некоторых работников с почасовой оплатой (один из потомков) и фиксированной оплатой (второй потомок).
            а) Описать в базовом классе абстрактный метод для расчёта среднемесячной заработной платы. 
                Для «повременщиков» формула для расчета такова: «среднемесячная заработная плата = 20.8 * 8 * почасовая ставка», 
                для работников с фиксированной оплатой «среднемесячная заработная плата = фиксированная месячная оплата».
            б) Создать на базе абстрактного класса массив сотрудников и заполнить его, вывести список сотрудников на экран.
        Домашняя работа посвящена теме урока "Порождающие паттерны"

        2*.
        a) Процедуру генерации сотрудников оформить с использованием паттерна "Фабричный метод"
        б) Механизм заполнения свойств работника (фамилия, имя, отчество, уровень заработной платы) оформить с использованием паттерна "строитель" (можно использовать методы расширения,
        как это было показано на уроке, можно с использованием "текучего интерфейса".
    */
    #endregion

    internal class Program
    {
        static void Main(string[] args)
        {
            NamesDB namesDB = new NamesDB();
            EmployeeCreator randomEmployeeCreator = new EmployeeCreator();
            List<Employee> employees = new List<Employee>();
            Random random = new Random();

            Console.Write("Enter number of employees: ");
            int.TryParse(Console.ReadLine(), out int numOfEmployees);
            Console.WriteLine();

            for (int i = 0; i < numOfEmployees; i++)
            {
                switch(random.Next(0, 4))
                {
                    case 0:
                        {
                            employees.Add(randomEmployeeCreator.NewEmployee(EmployeeType.PartTime, namesDB, 1000));
                            break;
                        }
                    case 3:
                        {
                            employees.Add(randomEmployeeCreator.NewEmployee(EmployeeType.PartTime, namesDB, 1000));
                            break;
                        }
                    default:
                        {
                            employees.Add(randomEmployeeCreator.NewEmployee(EmployeeType.FullTime, namesDB, 10000));
                            break;
                        }
                }
            }

            foreach(Employee employee in employees)
            {
                Console.WriteLine(employee);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.Write("Hit any key to exit..");
            Console.ReadKey();
        }
    }
}