using RabbitMQ.Client;

namespace ToDoListInfrastructure.Messaging
{
    public class RabbitConnection : IDisposable
    {
        #region protected properties

        protected IConnection? _connection;
        protected IModel? _rabbitModel;

        #endregion

        #region public properties

        public RabbitConnectionDescriptor ConnectionDescriptor { get; set; }

        #endregion

        #region ctor

        public RabbitConnection()
        {
            ConnectionDescriptor = new RabbitConnectionDescriptor();
            Connect();
        }
        #endregion

        #region IDisposable implementation

        #endregion
        public void Dispose()
        {
            _rabbitModel?.Close();
        }


        #region private methods

        private void Connect()
        {
            var fac = new ConnectionFactory { HostName = ConnectionDescriptor.ConnectionString };
            _connection = fac.CreateConnection();
            _rabbitModel = _connection.CreateModel();

            _rabbitModel.QueuePurge(ConnectionDescriptor.Queue);

            _rabbitModel.QueueDeclare(queue: ConnectionDescriptor.Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        #endregion
    }
}
