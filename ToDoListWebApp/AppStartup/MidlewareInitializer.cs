using System.Runtime.CompilerServices;

namespace ToDoListWebApp.AppStartup
{
    public static class MidlewareInitializer
    {
        #region public methods

        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            ConfigureSwagger(app);

            return app;
        }

        #endregion

        #region private methods

        private static void ConfigureSwagger(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        #endregion
    }
}
