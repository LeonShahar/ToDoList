using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ToDoListInfrastructure.Messaging;
using ToDoListInfrastructure.Model;
using ToDoListRepo.Interfaces;

namespace ToDoListRepo.Services
{
    internal sealed class ToDoListRepoService : IHostedService
    {
        #region private members

        private long _isRunning = 1;

        private readonly RabbitConsumer? _rabbitConsumer;
        private readonly IToDoListRepository? _databaseRepo;

        private readonly Dictionary<ToDoItemActionEnum, Func<ToDoItemAction, Task<string>>> _actionHandlers;
        #endregion

        #region ctor

        public ToDoListRepoService(IHostApplicationLifetime hostAppLifetime, RabbitConsumer rabbitConsumer, IToDoListRepository databaseRepo)
        {
            _rabbitConsumer = rabbitConsumer;
            hostAppLifetime.ApplicationStopping.Register(_rabbitConsumer.Dispose);

            object value = Task.Run(() => _rabbitConsumer.Start(RabbitMessageHandler));

            _actionHandlers = new Dictionary<ToDoItemActionEnum, Func<ToDoItemAction, Task<string>>>
            {
                {ToDoItemActionEnum.AddPerson, AddPerson },
                { ToDoItemActionEnum.AddToDoItem, AddToDoItem },
                { ToDoItemActionEnum.UpdateToDoItem, UpdateToDoItem},
                { ToDoItemActionEnum.ViewAllItems, ViewAllItems},
                { ToDoItemActionEnum.ViewPersons, ViewPersons },
                { ToDoItemActionEnum.DeletePerson, DeleteUser },
                { ToDoItemActionEnum.DeletePersonAllItems, DeleteUserAllItems },
                { ToDoItemActionEnum.GetPerson, GetPerson }
            };

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

        private async Task<string> RabbitMessageHandler(string message)
        {
            if (_databaseRepo == null)
                throw new NullReferenceException(nameof(_databaseRepo));

            var request = JsonConvert.DeserializeObject<ToDoItemAction>(message);

            if (request == null)
                return string.Empty;

            return await _actionHandlers[request.RequestAction](request);
        }

        private async Task<string> AddPerson(ToDoItemAction request)
        {
            var jsonPerson = string.Empty;

            await Task.Run(() =>
            {
                var person = _databaseRepo?.AddPerson(request.FirstName, request.LastName);

                jsonPerson = person != null ? JsonConvert.SerializeObject(person) : string.Empty;
            });

            return jsonPerson;
        }

        private async Task<string> AddToDoItem(ToDoItemAction request)
        {
            if (request == null)
                return string.Empty;

            return await Task.Run(() => _databaseRepo?.AddToDoItem(request.PersonId, request.ToDoItem) == true ? "Success" : "Failed");
        }

        private async Task<string> UpdateToDoItem(ToDoItemAction request)
        {
            return await Task.Run(() => string.Empty);
        }

        private async Task<string> ViewAllItems(ToDoItemAction request)
        {
            return await Task.Run(() => string.Empty);
        }

        private async Task<string> ViewPersons(ToDoItemAction request)
        {
            var jsonResponse = string.Empty;
            await Task.Run(() =>
            {
                var items = _databaseRepo?.GetAllPersons();
                jsonResponse = JsonConvert.SerializeObject(items, Formatting.Indented);
            });

            return jsonResponse;
        }

        private async Task<string> DeleteUser(ToDoItemAction request)
        {
            return await Task.Run(() => string.Empty);
        }

        async Task<string> DeleteUserAllItems(ToDoItemAction request)
        {
            return await Task.Run(() => string.Empty);
        }

        private async Task<string> GetPerson(ToDoItemAction request)
        {
            var jsonPerson = string.Empty;

            await Task.Run(() =>
            {
                var person = _databaseRepo?.GetPersonByName(request.FirstName, request.LastName);
                jsonPerson = JsonConvert.SerializeObject(person, Formatting.Indented);
            });

            return jsonPerson;
        }
        #endregion
    }
}
