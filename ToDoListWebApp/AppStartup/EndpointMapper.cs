namespace ToDoListWebApp.AppStartup
{
    public static class EndpointMapper
    {
        #region public methods

        public static WebApplication RegisterEndpoint(this WebApplication app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.MapControllers();

            return app;
        }

        #endregion
    }
}
