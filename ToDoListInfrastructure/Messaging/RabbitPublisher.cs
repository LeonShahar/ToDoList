using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace ToDoListInfrastructure.Messaging
{
    public sealed class RabbitPublisher : RabbitConnection
    {
        #region private members

        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new();

        #endregion

        #region ctor

        public RabbitPublisher() : base()
        {
            _rabbitModel.QueueDeclare(queue: ConnectionDescriptor.CallbackQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _rabbitModel.QueueBind(ConnectionDescriptor.CallbackQueue, ConnectionDescriptor.Excahnge, ConnectionDescriptor.CallbackRoutingKey);
            _rabbitModel.QueuePurge(ConnectionDescriptor.CallbackQueue);

            var consumer = new EventingBasicConsumer(_rabbitModel);

            consumer.Received += (model, ea) =>
            {
                if (!_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                    return;
                
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                tcs.TrySetResult(response);
            };

            _rabbitModel.BasicConsume(queue: ConnectionDescriptor.CallbackQueue, autoAck: false, consumer: consumer);
        }

        public Task<string> SendMessageAsync(string message, CancellationToken cancellationToken = default)
        {
            if (_rabbitModel == null)
                throw new NullReferenceException(nameof(_rabbitModel));

            IBasicProperties props = _rabbitModel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.Persistent = false;
            props.CorrelationId = correlationId;
            props.ReplyTo = ConnectionDescriptor.CallbackQueue;
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var tcs = new TaskCompletionSource<string>();
            _callbackMapper.TryAdd(correlationId, tcs);

            _rabbitModel.BasicPublish(exchange: ConnectionDescriptor.Excahnge,
                                 routingKey: ConnectionDescriptor.RoutingKey,
                                 basicProperties: props,
                                 body: messageBytes);

            cancellationToken.Register(() => _callbackMapper.TryRemove(correlationId, out _));

            return cancellationToken == default ? tcs.Task : tcs.Task.WaitAsync(cancellationToken);
        }

        #endregion
    }
}
