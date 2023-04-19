namespace ToDoListInfrastructure.Messaging
{
    public sealed class RabbitConnectionDescriptor
    {
        #region public properties

        public string ConnectionString { get; } = "localhost";

        public string Excahnge { get; } = "ToDoListExchenge";

        public string Queue { get; } = "ToDoList";

        public string RoutingKey { get; } = "ToDoList";

        #endregion
    }
}
