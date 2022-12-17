using System.Security.Cryptography.X509Certificates;

namespace Lesson_02
{
    #region Задача
    /*
        Задачу необходимо выполнить в рамках многопоточного программирования, потоки необходимо запускать на пулле потоков (ThreadPool).
        Имеется пустой участок земли (двумерный массив)

        private static char[,] field;
        и план поля

        Console.Write("Укажите размер поля по оси X: ");
        x = Convert.ToInt32(Console.ReadLine());
        Console.Write("Укажите размер поля по оси Y: ");
        y = Convert.ToInt32(Console.ReadLine());
        field = new char[y, x];
        поле необходимо засеять.
        Эту задачу выполняют два фермера, которые не хотят встречаться друг с другом.
        Первый фермер начинает работу с верхнего левого угла поля и перемещается слева направо, сделав ряд, он спускается вниз.

        X X X . . .     X X X X X .     X X X X X X     X X X X X X     
        . . . . . .     . . . . . .     X . . . . .     X X X . . .     
        . . . . . .     . . . . . .     . . . . . .     . . . . . .     
        . . . . . .     . . . . . .     . . . . . .     . . . . . .     
        Второй фермер начинает работу с нижнего правого угла поля и перемещается снизу вверх, сделав ряд, он перемещается влево.

        . . . . . .     . . . . . 0     . . . . . 0     . . . . . 0     
        . . . . . 0     . . . . . 0     . . . . . 0     . . . . 0 0     
        . . . . . 0     . . . . . 0     . . . . 0 0     . . . . 0 0     
        . . . . . 0     . . . . . 0     . . . . 0 0     . . . . 0 0     
        После очередного посева поля, каждый из фермеров немного отдыхает:
        ...
        field[i, j] = 'X';
        Thread.Sleep(10);
        ...
        Если фермер видит, что участок поля уже засеян другим фермером, он идет дальше. Фермеры должны работать параллельно.

        Задача: Создать многопоточное приложение, моделирующее работу фермеров, дождаться завершения выполнения работы фермеров, вывести засеянное
        поле на экран. 
     */
    #endregion

    /// <summary>
    /// Тип фермера
    /// </summary>
    public enum FarmerType
    {
        Farmer1,
        Farmer2,
    }

    /// <summary>
    /// Поле
    /// </summary>
    public class FieldResource
    {
        public char[,]? Field { get; set; }

        public int FieldLines {
            get
            {
                if (Field == null) return 0;
                return Field.GetUpperBound(0) + 1;
            }
        }

        public int FieldColumns
        {
            get
            {
                if (Field == null) return 0;
                return Field.Length / FieldLines;
            }
        }

        public FieldResource(char[,]? field)
        {
            Field = field;
            this.PlowField();
        }

        /// <summary>
        /// Метод инициализации поля (вспахивание)
        /// </summary>
        public void PlowField()
        {
            if (Field != null)
            {
                lock (Field)
                {
                    for (int i = 0; i < FieldLines; i++)
                    {
                        for (int y = 0; y < FieldColumns; y++)
                        {
                            Field[i,y] = '-';
                        }
                    }
                }
            }
        }

        public void PrintField()
        {
            if (Field != null)
            {
                for (int i = 0; i < FieldLines; i++)
                {
                    Console.WriteLine();
                    for (int y = 0; y < FieldColumns; y++)
                    {
                        Console.Write(Field[i, y]);
                    }
                }

                Console.WriteLine();
            }
        }
    }

    public class FarmerThreadControl
    {
        public FieldResource Field { get; }
        public AutoResetEvent WaitHandle { get; }
        public FarmerType Farmer { get;  }
        public FarmerThreadControl(FieldResource field, AutoResetEvent waitHandle, FarmerType farmer)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
            WaitHandle = waitHandle ?? throw new ArgumentNullException(nameof(waitHandle));
            Farmer = farmer;
        }
    }

    internal class Program
    {
        private static FieldResource field;

        static void Main(string[] args)
        {
            Console.Write("Укажите размер поля по оси X: ");
            Int32.TryParse(Console.ReadLine(), out int x);
            Console.Write("Укажите размер поля по оси Y: ");
            Int32.TryParse(Console.ReadLine(), out int y);

            if (x > 0 && y > 0)
            {
                field = new FieldResource(new char[y, x]);

                AutoResetEvent[] autoResetEvents = new AutoResetEvent[2];

                autoResetEvents[0] = new AutoResetEvent(false);
                autoResetEvents[1] = new AutoResetEvent(false);

                ThreadPool.QueueUserWorkItem(new WaitCallback(FarmerTask), new FarmerThreadControl(field, autoResetEvents[0], FarmerType.Farmer1));
                ThreadPool.QueueUserWorkItem(new WaitCallback(FarmerTask), new FarmerThreadControl(field, autoResetEvents[1], FarmerType.Farmer2));
                WaitHandle.WaitAll(autoResetEvents);

                field.PrintField();
            }

            Console.WriteLine();
            Console.Write("Press any key to exit..");
            Console.ReadKey(true);
        }

        public static void FarmerTask(object? data)
        {
            if (data != null && data is FarmerThreadControl)
            {
                FarmerThreadControl taskData = (FarmerThreadControl)data;
                int fieldLines = taskData.Field.FieldLines;
                int fieldColumns = taskData.Field.FieldColumns;
                bool FarmerNeedBreak;

                switch (taskData.Farmer)
                {
                    case FarmerType.Farmer1:
                        {
                            for (int i = 0; i < fieldLines; i++)
                            {
                                for (int y = 0; y < fieldColumns; y++)
                                {
                                    FarmerNeedBreak = false;

                                    lock (taskData.Field)
                                    {
                                        if (taskData.Field.Field[i, y] == '-')
                                        {
                                            FarmerNeedBreak = true;
                                            taskData.Field.Field[i, y] = 'X';
                                            //Console.WriteLine("x");
                                            //Console.Clear();
                                            //taskData.Field.PrintField();
                                        }

                                    }

                                    // Отдыхаем только если фермер сеял.
                                    if (FarmerNeedBreak) Thread.Sleep(10);
                                }
                            }
                            break;
                        }
                    case FarmerType.Farmer2:
                        {
                            for (int y = fieldColumns - 1; y >= 0; y--)
                            {
                                for (int i = fieldLines - 1; i >= 0; i--)
                                {
                                    FarmerNeedBreak = false;

                                    lock (taskData.Field)
                                    {
                                        if (taskData.Field.Field[i, y] == '-')
                                        {
                                            FarmerNeedBreak = true;
                                            taskData.Field.Field[i, y] = '0';
                                            //Console.WriteLine("0");
                                            //Console.Clear();
                                            //taskData.Field.PrintField();
                                        }
                                    }

                                    // Отдыхаем только если фермер сеял.
                                    if (FarmerNeedBreak) Thread.Sleep(10);
                                }
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                taskData.WaitHandle.Set();
            }
        }


        public static void Farmer1Task(object? data)
        {
            if (data != null && data is FarmerThreadControl)
            {
                FarmerThreadControl taskData = (FarmerThreadControl)data;
                int fieldLines = taskData.Field.FieldLines;
                int fieldColumns = taskData.Field.FieldColumns;
                bool FarmerNeedBreak;

                for (int i = 0; i < fieldLines; i++)
                {
                    for (int y = 0; y < fieldColumns; y++)
                    {
                        FarmerNeedBreak = false;

                        lock (taskData.Field)
                        {
                            if (taskData.Field.Field[i, y] == '-')
                            {
                                FarmerNeedBreak = true;
                                taskData.Field.Field[i, y] = 'X';
                                //Console.WriteLine("x");
                                //Console.Clear();
                                //taskData.Field.PrintField();
                            }

                        }

                        // Отдыхаем только если фермер сеял.
                        if (FarmerNeedBreak) Thread.Sleep(10);
                    }
                }

                taskData.WaitHandle.Set();
            }
        }

        public static void Farmer2Task(object? data)
        {
            if (data != null && data is FarmerThreadControl)
            {
                FarmerThreadControl taskData = (FarmerThreadControl)data;
                int fieldLines = taskData.Field.FieldLines;
                int fieldColumns = taskData.Field.FieldColumns;
                bool FarmerNeedBreak;

                for (int y = fieldColumns - 1; y >= 0; y--)
                {
                    for (int i = fieldLines - 1; i >= 0; i--)
                    {
                        FarmerNeedBreak = false;

                        lock (taskData.Field)
                        {
                            if (taskData.Field.Field[i, y] == '-')
                            {
                                FarmerNeedBreak = true;
                                taskData.Field.Field[i, y] = '0';
                                //Console.WriteLine("0");
                                //Console.Clear();
                                //taskData.Field.PrintField();
                            }
                        }

                        // Отдыхаем только если фермер сеял.
                        if (FarmerNeedBreak) Thread.Sleep(10);
                    }
                }

                taskData.WaitHandle.Set();
            }
        }
    }
}