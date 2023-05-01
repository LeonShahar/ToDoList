using ToDoListWebApp.AppStartup;

namespace ToDoListWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.RegisterApplicationServices();

            builder
                .Build()
                .ConfigureMiddleware()
                .RegisterEndpoint()
                .Run();
        }
    }
}