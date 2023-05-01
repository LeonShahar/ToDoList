using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ToDoListInfrastructure.Messaging
{
    public  class RabbitConsumer : RabbitConnection
    {
        #region private members
        
        private Func<string, Task<string>>? _messageHandler;

        #endregion

        #region ctor

        public RabbitConsumer() : base()
        {
            if (_rabbitModel == null)
                throw new NullReferenceException(nameof(_rabbitModel));

            _rabbitModel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_rabbitModel);
            _rabbitModel.BasicConsume(queue: ConnectionDescriptor.Queue,
                                consumerTag: $"{ConnectionDescriptor.RoutingKey}_Tag",
                                noLocal: false,
                                exclusive: false,
                                autoAck: false,
                                arguments: null,
                                consumer: consumer);


            consumer.Received += Consumer_Received;
        }

        #endregion

        #region public methods

        public void Start(Func<string, Task<string>> messageHandler)
        {
            _messageHandler = messageHandler;
        }

        #endregion

        #region private methods

        private void Consumer_Received(object? sender, BasicDeliverEventArgs ea)
        {
            if (_rabbitModel == null)
                throw new NullReferenceException(nameof(_rabbitModel));

            if (_messageHandler == null)
                throw new NullReferenceException(nameof(_messageHandler));

            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;
            var replyProps = _rabbitModel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            var responseMessage = string.Empty;
            try
            {
                var message = Encoding.UTF8.GetString(body);
                responseMessage = _messageHandler.Invoke(message).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine($" [.] {e.Message}");
                responseMessage = string.Empty;
            }
            finally
            {
                var responseBytes = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(responseMessage) ? "{}" : responseMessage);

                _rabbitModel.BasicPublish(exchange: ConnectionDescriptor.Excahnge,
                    routingKey: ConnectionDescriptor.CallbackRoutingKey,
                    basicProperties: replyProps,
                    body: responseBytes);

                _rabbitModel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
        }

        #endregion
    }
}
