namespace Lesson_01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task1();
            Task2();
            Task3();
            Console.Write("Hit any key to exit ...");
            Console.ReadKey(true);
        }

        public static void Task1()
        {
            float[] arr = new float[100_000_000];
            System.Array.Fill(arr, 1);

            DateTime start = DateTime.Now;

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (float)(arr[i] * Math.Sin(0.2f + i / 5) * Math.Cos(0.2f + i / 5) *
                    Math.Cos(0.4f + i / 2));
            }

            DateTime finish = DateTime.Now;

            Console.WriteLine(arr[99_999_999]);
            Console.WriteLine($"Task1: Инициализация массива заняла у нас:  {finish - start} сек.");
        }

        public static void Task2()
        {
            float[] arr = new float[100_000_000];
            System.Array.Fill(arr, 1);

            float[] tmpArray1 = new float[50_000_000];
            float[] tmpArray2 = new float[50_000_000];

            System.Array.Copy(arr, 0, tmpArray1, 0, 50_000_000);
            System.Array.Copy(arr, 50_000_000, tmpArray2, 0, 50_000_000);

            AutoResetEvent[] autoResetEvents = new AutoResetEvent[2];
            for (int i = 0; i < autoResetEvents.Length; i++)
            {
                autoResetEvents[i] = new AutoResetEvent(false);
            }

            DateTime start = DateTime.Now;

            Thread task1 = new Thread(() =>
            {
                for (int i = 0; i < tmpArray1.Length; i++)
                {
                    tmpArray1[i] = (float)(tmpArray1[i] * Math.Sin(0.2f + i / 5) * Math.Cos(0.2f + i / 5) *
                        Math.Cos(0.4f + i / 2));
                }

                autoResetEvents[0].Set();

            });

            Thread task2 = new Thread(() =>
            {
                for (int i = 0; i < tmpArray2.Length; i++)
                {
                    tmpArray2[i] = (float)(tmpArray2[i] * Math.Sin(0.2f + (i + 50_000_000) / 5) * Math.Cos(0.2f + (i + 50_000_000) / 5) *
                        Math.Cos(0.4f + (i + 50_000_000) / 2));
                }

                autoResetEvents[1].Set();

            });

            task1.Start();
            task2.Start();
            AutoResetEvent.WaitAll(autoResetEvents);

            System.Array.Copy(tmpArray1, 0, arr, 0, 50_000_000);
            System.Array.Copy(tmpArray2, 0, arr, 50_000_000, 50_000_000);

            DateTime finish = DateTime.Now;

            Console.WriteLine(arr[99_999_999]);
            Console.WriteLine($"Task2: Инициализация массива заняла у нас:  {finish - start} сек.");
        }

        public static void Task3()
        {
            float[] arr = new float[100_000_000];
            System.Array.Fill(arr, 1);

            TaskData[] taskDatas = new TaskData[2];

            for (int i = 0; i < taskDatas.Length; i++)
            {
                taskDatas[i] = new TaskData();
                float[] tmpArray = new float[50_000_000];
                System.Array.Copy(arr, i * 50_000_000, tmpArray, 0, 50_000_000);
                taskDatas[i].Data = tmpArray;
                taskDatas[i].Offset = i * 50_000_000;
            }

            AutoResetEvent[] autoResetEvents = new AutoResetEvent[taskDatas.Length];
            for (int i = 0; i < autoResetEvents.Length; i++)
            {
                autoResetEvents[i] = taskDatas[i].ResetEvent;
            }

            DateTime start = DateTime.Now;

            for (int i = 0; i < autoResetEvents.Length; i++)
            {
                Thread task = new Thread((obj) =>
                {
                    if (obj != null && obj is TaskData)
                    {
                        TaskData taskData = (TaskData)obj;
                        int y = 0;
                        for (int i = 0; i < taskData.Data.Length; i++)
                        {
                            y = taskData.Offset + i;
                            taskData.Data[i] = (float)(taskData.Data[i] * Math.Sin(0.2f + y / 5) * Math.Cos(0.2f + y / 5) *
                                Math.Cos(0.4f + y / 2));
                        }

                        taskData.ResetEvent.Set();
                    }
                });

                task.Start(taskDatas[i]);
            }

            AutoResetEvent.WaitAll(autoResetEvents);

            for (int i = 0; i < taskDatas.Length; i++)
            {
                System.Array.Copy(taskDatas[i].Data, 0, arr, i * 50_000_000, 50_000_000);
            }

            DateTime finish = DateTime.Now;

            Console.WriteLine(arr[99_999_999]);
            Console.WriteLine($"Task3: Инициализация массива заняла у нас:  {finish - start} сек.");
        }

        public class TaskData
        {
            public float[] Data { get; set; }

            public int Offset { get; set; }
            public AutoResetEvent ResetEvent { get; set; }

            public TaskData()
            {
                Offset = 0;
                ResetEvent = new AutoResetEvent(false);
            }
        }
    }

}