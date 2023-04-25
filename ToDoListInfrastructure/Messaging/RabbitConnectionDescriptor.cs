namespace ToDoListInfrastructure.Messaging
{
    public sealed class RabbitConnectionDescriptor
    {
        #region public properties

        public string ConnectionString { get; } = "localhost";

        public string Excahnge { get; } = "ToDoListExchenge";

        public string Queue { get; } = "ToDoList";

        public string CallbackQueue { get; } = "ToDoListCallback";

        public string RoutingKey { get; } = "ToDoList";

        public string CallbackRoutingKey { get; } = "ToDoList_Callback";

        #endregion
    }
}
