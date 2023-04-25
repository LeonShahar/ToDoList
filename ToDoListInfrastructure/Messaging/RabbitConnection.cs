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
            var fac = new ConnectionFactory 
            { 
                HostName = ConnectionDescriptor.ConnectionString,
                UserName = "guest",
                Password = "guest",
            };

            Console.WriteLine($"Connecting to RabbitMQ at: '{fac.HostName}' as {fac.UserName}-{fac.Password}");
            _connection = fac.CreateConnection();
            _rabbitModel = _connection.CreateModel();

            _rabbitModel.ExchangeDeclare(ConnectionDescriptor.Excahnge, ExchangeType.Direct, true);
            Console.WriteLine($"Created RabbitMQ Exchange: '{ConnectionDescriptor.Excahnge}'");

            _rabbitModel.QueueDeclare(queue: ConnectionDescriptor.Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            Console.WriteLine($"Created RabbitMQ Queue: '{ConnectionDescriptor.Queue}'");

            Console.WriteLine($"Binding Queue: '{ConnectionDescriptor.Queue}' to Exchange: '{ConnectionDescriptor.Excahnge}' WithRoutingKey: '{ConnectionDescriptor.RoutingKey}'");
            _rabbitModel.QueueBind(ConnectionDescriptor.Queue, ConnectionDescriptor.Excahnge, ConnectionDescriptor.RoutingKey);
            _rabbitModel.QueuePurge(ConnectionDescriptor.Queue);
        }

        #endregion
    }
}
