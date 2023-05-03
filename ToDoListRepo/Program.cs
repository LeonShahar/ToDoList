using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoListInfrastructure.Messaging;
using ToDoListInfrastructure.Model;
using ToDoListRepo.Interfaces;
using ToDoListRepo.Repositories;
using ToDoListRepo.Services;

namespace ToDoListRepo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host =  CreateHostBuilder(args).Build();

            var ct = new CancellationTokenSource();
            var service = host.Services.GetRequiredService<ToDoListRepoService>();

            service.StartAsync(ct.Token);
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile(Path.Combine("Properties", "appsettings.json"));
                })
                .ConfigureServices((hostContext, services) => 
                    services
                        .AddSingleton<ToDoListDbContext>()
                        .AddSingleton<IToDoListRepository, ToDoListRepository>()
                        .AddDbContext<ToDoListDbContext>(options => options.UseInMemoryDatabase("ToDoListItems"))
                        .AddSingleton<RabbitConsumer>(/*new RabbitConsumer()*/)
                        .AddHostedService<ToDoListRepoService>())
                        .ConfigureServices(services => services.AddSingleton<ToDoListRepoService>());
        }
    }
}