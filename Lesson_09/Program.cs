using Lesson09.DAL;
using Lesson09.DAL.Entities;
using Lesson09.Services;
using Lesson09.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Lesson09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host
                .ConfigureAppConfiguration(app => 
                    app.AddJsonFile("appsettings.json"))
                .ConfigureLogging(options =>
                    options.ClearProviders()
                        .AddConsole()
                        .AddDebug()
                );

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();
            builder.Services.AddDbContext<Lesson09DbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var servicesScope = app.Services.CreateAsyncScope())
            {
                var services = servicesScope.ServiceProvider;
                var dbContext = services.GetRequiredService<Lesson09DbContext>();
                dbContext.Database.Migrate();
            }

            app.Run();

        }
    }
}