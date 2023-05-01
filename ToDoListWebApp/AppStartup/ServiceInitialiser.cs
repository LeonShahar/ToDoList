using ToDoListInfrastructure.Messaging;
using ToDoListWebApp.Interfaces;
using ToDoListWebApp.Managers;

namespace ToDoListWebApp.AppStartup
{
    public static class ServiceInitialiser
    {
        #region public methods

        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            ConfigureServices(services);

            services.AddControllers();
            RegisterSwagger(services);

            return services;
        }

        #endregion

        #region private methods

        private static void ConfigureServices(IServiceCollection services) 
        {
            services.AddSingleton<RabbitPublisher>();
            services.AddSingleton<IToDoListManager, ToDoListManager>();
        }

        private static void RegisterSwagger(IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        #endregion
    }
}
