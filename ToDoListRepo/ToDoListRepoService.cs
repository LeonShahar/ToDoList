using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ToDoListInfrastructure.Messaging;
using ToDoListInfrastructure.Model;

namespace ToDoListRepo
{
    internal sealed class ToDoListRepoService : IHostedService
    {
        #region private members

        private long _isRunning = 1;

        private readonly RabbitConsumer? _rabbitConsumer;
        private readonly IToDoListRepository? _databaseRepo;
        #endregion

        #region ctor

        public ToDoListRepoService(RabbitConsumer rabbitConsumer, IToDoListRepository databaseRepo) 
        { 
            _rabbitConsumer = rabbitConsumer;
            _rabbitConsumer.Start(RabbitMessageHandler);

            _databaseRepo = databaseRepo;
        }

        #endregion

        #region IHostedService implementation

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Starting {nameof(ToDoListRepoService)} ...");
            while (Interlocked.Read(ref _isRunning) == 1)
            {
                var key = Console.ReadKey(true);
                if (key.KeyChar == 'q' || key.KeyChar == 'Q')
                    return StopAsync(cancellationToken);

            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Interlocked.Exchange(ref _isRunning, 0);
            return Task.CompletedTask;
        }

        #endregion

        #region private methods

        private string RabbitMessageHandler(string message)
        {
            if (_databaseRepo == null)
                throw new NullReferenceException(nameof(_databaseRepo));


            var items = _databaseRepo.GetAllPersons();

            var jsonResponse = JsonConvert.SerializeObject(items, Formatting.Indented);

            return jsonResponse;
        }
        #endregion
    }
}
