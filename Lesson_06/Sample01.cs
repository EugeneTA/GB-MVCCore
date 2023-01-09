using Microsoft.EntityFrameworkCore;
using Orders.DAL;
using Orders.DAL.Entities;

namespace Lesson_06
{
    internal class Sample01
    {
        static void Main(string[] args)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseSqlServer("data source=NB-TISHKOV;initial catalog=OrdersDatabase;User Id=OrdersDatabaseUser;Password=12345;MultipleActiveResultSets=True;App=EntityFramework");


            using (var context = new OrdersDbContext(dbContextOptionsBuilder.Options))
            {
                context.Database.EnsureCreated();

                // https://randomus.ru/name

                if (!context.Buyers.Any())
                {
                    context.Buyers.Add(new Buyer
                    {
                        LastName = "Трофимов",
                        Name = "Алексей",
                        Patronymic = "Артёмович",
                        Birthday = DateTime.Now.AddYears(-23).Date,
                    });
                    context.Buyers.Add(new Buyer
                    {
                        LastName = "Зеленин",
                        Name = "Николай",
                        Patronymic = "Даниилович",
                        Birthday = DateTime.Now.AddYears(-36).Date,
                    });
                    context.Buyers.Add(new Buyer
                    {
                        LastName = "Ермаков",
                        Name = "Фёдор",
                        Patronymic = "Дмитриевич",
                        Birthday = DateTime.Now.AddYears(-19).Date,
                    });
                    context.Buyers.Add(new Buyer
                    {
                        LastName = "Смирнова",
                        Name = "Ангелина",
                        Patronymic = "Данииловна",
                        Birthday = DateTime.Now.AddYears(-31).Date,
                    });
                    context.Buyers.Add(new Buyer
                    {
                        LastName = "Белоусова",
                        Name = "Мария",
                        Patronymic = "Денисовна",
                        Birthday = DateTime.Now.AddYears(-26).Date,
                    });

                    context.SaveChanges();
                }

                foreach (var buyer in context.Buyers)
                {
                    Console.WriteLine($"{buyer.LastName} {buyer.Name} {buyer.Patronymic} {buyer.Birthday.ToShortDateString()}");
                }

                Console.ReadKey();

            }

        }
    }
}