using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using Lesson_07.Models.Reports;
using Lesson_07.Services;
using Lesson_07.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.DAL;
using Orders.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lesson_07.Extensions;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.Extensions.Options;

namespace Lesson_07
{

    // Добавить пакеты: [1] TemplateEngine.Docx
    internal class Program
    {
        private static Random random = new Random();

        private static IHost? _host;
        public static IServiceProvider Services => Hosting.Services;
        public static IHost Hosting => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(options =>
                    options.AddJsonFile("appsettings.json"))
                .ConfigureAppConfiguration(options =>
                    options
                        .AddJsonFile("appsettings.json")
                        .AddXmlFile("appsettings.xml", true)
                        .AddIniFile("appsettings.ini", true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args))
                .ConfigureLogging(options =>
                    options.ClearProviders() // Microsoft.Extensions.Logging
                        .AddConsole()
                        .AddDebug())
                .ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            //#region Register Base Services

            //// Стандартный способ регистрации сервиса (Microsoft.Extensions.DependencyInjection)
            //services.AddTransient<IOrderService, OrderService>();

            //#endregion


            #region Configure EF DBContext Service

            services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseSqlServer(host.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });

            #endregion
        }



        static async Task Main(string[] args)
        {
            var host = Hosting;
            await host.StartAsync();
            await PrintBuyersAsync();
            //Console.ReadKey(true);
            await host.StopAsync();
        }

        private static async Task PrintBuyersAsync()
        {
            await using (var servicesScope = Services.CreateAsyncScope())
            {
                var services = servicesScope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<Program>>();
                var context = services.GetRequiredService<OrdersDbContext>();

                context.Database.EnsureCreated();

                await context.Database.MigrateAsync(); 

                foreach (var buyer in context.Buyers)
                {
                    logger.LogInformation($"Покупатель >>> {buyer.Id} {buyer.LastName} {buyer.Name} {buyer.Patronymic} {buyer.Birthday.ToShortDateString()}");
                }

                foreach (var ord in context.Orders)
                {
                    logger.LogInformation($"Покупатель >>> {ord.Id} {ord.Address} {ord.Phone} {ord.OrderDate} {ord.Buyer.Name} {ord.Buyer.LastName} {ord.Items.Count}");
                }

                foreach (var prod in context.Products)
                {
                    logger.LogInformation($"Товар >>> {prod.Id} {prod.Name} {prod.Category} {prod.Price}");
                }

                //var orderService = services.GetRequiredService<IOrderService>();


                //await orderService.CreateAsync(random.Next(1, 6), "123, Russia, Address", "+79001112233", new (int, int)[] {
                //    new ValueTuple<int, int>(1, 1),
                //    new ValueTuple<int, int>(2, 2)
                //});


                //var catalog = new ProductsCatalog
                //{
                //    Name = "Каталог товаров",
                //    Description = "Актуальный список товаров на дату",
                //    CreationDate = DateTime.Now,
                //    Products = context.Products
                //};

                //string templateFile = "Templates/DefaultTemplate.docx";
                //IProductReport report = new ProductReportWord(templateFile);

                //CreateReport(report, catalog, "Report.docx");

                Console.WriteLine("Список заказов:");
                foreach(var orderInf in context.Orders)
                {
                    Console.WriteLine($"#:{orderInf.Id} - Покупатель: {orderInf.Buyer.Name} {orderInf.Buyer.LastName}");
                }

                Console.Write("Select order number to generate report: ");
                int.TryParse(Console.ReadLine(), out int orderNumber);

                var order =  context.Orders.FirstOrDefault(o => o.Id == orderNumber);

                if (order != null)
                {
                    Seller seller = new Seller
                    {
                        Name = "ООО .\'Рога и копыта\'",
                        SellerAddress = new Address
                        {
                            Country = "Россия",
                            PostalCode = "123000",
                            Region = "Moscow",
                            City = "Moscow",
                            Street = "Arbat street",
                            Building = "4b",
                            Office = "123"
                        }
                    };

                    OrderInfo orderInfo = new OrderInfo
                    {
                        Seller = seller,
                        OrderNumber = orderNumber,
                        OrderDateTime = order.OrderDate,
                        Address = order.Address,
                        Phone = order.Phone,
                        Buyer = order.Buyer,
                        Items = order.Items.ToList()
                    };

                    string templateFile = "Templates/OrderTemplate.docx";
                    IOrderReport report = new OrderReportWord(templateFile);
                    CreateOrderReport(report, orderInfo, "OrderReport.docx");
                }
                else
                {
                    Console.WriteLine("Нет такого заказа");
                }

                Console.WriteLine("Нажмите клавишу для завершения работы ...");
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportGenerator">Объект - генератор отчета</param>
        /// <param name="catalog">Объект с данными</param>
        /// <param name="reportFileName">Наименование файла-отчета</param>
        private static void CreateReport(IProductReport reportGenerator, ProductsCatalog catalog, string reportFileName)
        {
            reportGenerator.CatalogName = catalog.Name;
            reportGenerator.CatalogDescription = catalog.Description;
            reportGenerator.CreationDate = catalog.CreationDate;
            reportGenerator.Products = catalog.Products.Select(product => (product.Id, product.Name, product.Category, product.Price));

            var reportFileInfo = reportGenerator.Create(reportFileName);
            reportFileInfo.Execute();
        }


        /// <summary>
        /// Создание отчета по заказу в формате Microsoft Word
        /// </summary>
        /// <param name="reportGenerator">Объект - генератор отчета</param>
        /// <param name="orderInfo">Объект с данными</param>
        /// <param name="reportFileName">Наименование файла-отчета</param>
        /// <exception cref="ArgumentNullException"></exception>
        private static void CreateOrderReport(IOrderReport reportGenerator, OrderInfo orderInfo, string reportFileName)
        {
            if (reportGenerator == null)
            {
                throw new ArgumentNullException("Report generator not defined");
            }
            if (orderInfo == null)
            {
                throw new ArgumentNullException("Order info not defined");
            }

            reportGenerator.SellerAddress = orderInfo.Seller.SellerAddress;
            reportGenerator.OrderNumber = orderInfo.OrderNumber;
            reportGenerator.OrderDateTime = orderInfo.OrderDateTime;
            reportGenerator.Buyer = orderInfo.Buyer;
            reportGenerator.Address = orderInfo.Address;
            reportGenerator.Phone = orderInfo.Phone;
            if (orderInfo.Items != null)
            {
                reportGenerator.OrderProducts = orderInfo.Items.Select(product => (product.ProdItem.Name, product.Quantity, product.ProdItem.Price));
            }
            
            var reportFileInfo = reportGenerator.Create(reportFileName);
            reportFileInfo.Execute();
        }

    }

}