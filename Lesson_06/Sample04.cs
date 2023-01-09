using Autofac;
using Autofac.Extensions.DependencyInjection;
using Lesson_06.Services.Impl;
using Lesson_06.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lesson_06.Autofac;
using System.Reflection;
using Autofac.Configuration;
using Orders.DAL.Entities;

namespace Lesson_06
{
    internal class Sample04
    {
        private static IHost? _host;

        public static IHost Hosting => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(container => // Autofac
                {

                    //container.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();    // Способ 1. Ркгистрация конкретного сервиса
                    //container.RegisterType<OrderService>().InstancePerLifetimeScope();                        // Способ 2.
                    //container.RegisterModule<ServicesModule>();                                               // Способ 3. Регистрация сервисов через модуль
                    //container.RegisterAssemblyModules(Assembly.GetCallingAssembly());                         // Способ 4. Регистрация всех модулей в сборке

                    var config = new ConfigurationBuilder()                                                     // Способ 5. Регистрация через Autofac Confog. Пакет Autofac.Configuration
                    .AddJsonFile("autofac.config.json", false, false);
                    //.AddXmlFile("autofac.config.xml", false, false);

                    var module = new ConfigurationModule(config.Build());
                    container.RegisterModule(module);

                })
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
        }

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            #region Configure EF DBContext Service

            services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseSqlServer(host.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });

            #endregion
        }

        //public static IServiceProvider Services
        //{
        //    get
        //    {
        //        return Hosting.Services;
        //    }
        //}

        public static IServiceProvider Services => Hosting.Services;

        static async Task Main(string[] args)
        {
            var host = Hosting;
            await host.StartAsync();
            await PrintBuyersAsync();
            //await AddProduct();
            Console.ReadKey(true);
            await host.StopAsync();
        }

        private static Random random = new Random();

        private static async Task PrintBuyersAsync()
        {
            await using var servicesScope = Services.CreateAsyncScope();
            var services = servicesScope.ServiceProvider;

            var context = services.GetRequiredService<OrdersDbContext>();
            var logger = services.GetRequiredService<ILogger<Sample04>>();

            foreach (var buyer in context.Buyers)
            {
                logger.LogInformation($"Покупатель >>> {buyer.LastName} {buyer.Name} {buyer.Patronymic} {buyer.Birthday.ToShortDateString()}");
            }

            var orderService = services.GetRequiredService<IOrderService>();

            await orderService.CreateAsync(random.Next(1, 6), "123, Russia, Address", "+79001112233", new (int, int)[] {
                    new ValueTuple<int, int>(1, 1),
                    new ValueTuple<int, int>(3, 2)
                });
        }


        private static async Task AddProduct()
        {
            await using var servicesScope = Services.CreateAsyncScope();
            var services = servicesScope.ServiceProvider;

            var context = services.GetRequiredService<OrdersDbContext>();
            var logger = services.GetRequiredService<ILogger<Sample04>>();

            Console.WriteLine("Enter product name");
            string productName = Console.ReadLine();

            Console.WriteLine("Enter category name");
            string productCategory = Console.ReadLine();

            Console.WriteLine("Enter price");
            Decimal.TryParse(Console.ReadLine(), out decimal productPrice);

            var productService = services.GetRequiredService<IProductService>();

            await productService.AddProductAsync(productName, productCategory, productPrice);

            foreach(var product in context.Products)
            {
                Console.WriteLine($"{product.Name} - {product.Category} - {product.Price}");
            }
        }
    }
}
