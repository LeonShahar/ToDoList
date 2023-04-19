using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace ToDoListInfrastructure.Messaging
{
    public sealed class RabbitPublisher : RabbitConnection
    {
        #region private members

        private readonly string _replyQueueName;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new();

        #endregion

        #region ctor

        public RabbitPublisher() : base()
        {
            var consummer = new EventingBasicConsumer(_rabbitModel);
            _rabbitModel.BasicConsume(queue: ConnectionDescriptor.Queue, autoAck: false, consumer: consummer);

            _replyQueueName = _rabbitModel.QueueDeclare().QueueName;

            var consumer = new EventingBasicConsumer(_rabbitModel);
            consumer.Received += (model, ea) =>
            {
                if (!_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                    return;
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                tcs.TrySetResult(response);
            };

            _rabbitModel.BasicConsume(consumer: consumer,
                                 queue: _replyQueueName,
                                 autoAck: true);
        }

        public Task<string> SendMessageAsync(string message, CancellationToken cancellationToken = default)
        {
            if (_rabbitModel == null)
                throw new NullReferenceException(nameof(_rabbitModel));

            IBasicProperties props = _rabbitModel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = _replyQueueName;
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var tcs = new TaskCompletionSource<string>();
            _callbackMapper.TryAdd(correlationId, tcs);

            _rabbitModel.BasicPublish(exchange: ConnectionDescriptor.Excahnge,
                                 routingKey: ConnectionDescriptor.RoutingKey,
                                 basicProperties: props,
                                 body: messageBytes);

            cancellationToken.Register(() => _callbackMapper.TryRemove(correlationId, out _));

            return tcs.Task;
        }

        #endregion

        #region public methods

        #endregion
    }
}
