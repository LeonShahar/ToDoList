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

        private readonly Dictionary<ToDoItemActionEnum, Func<ToDoItemAction, string>> _actionHandlers;
        #endregion

        #region ctor

        public ToDoListRepoService(RabbitConsumer rabbitConsumer, IToDoListRepository databaseRepo) 
        { 
            _rabbitConsumer = rabbitConsumer;
            _rabbitConsumer.Start(RabbitMessageHandler);

            _actionHandlers = new Dictionary<ToDoItemActionEnum, Func<ToDoItemAction, string>>
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

        private string RabbitMessageHandler(string message)
        {
            if (_databaseRepo == null)
                throw new NullReferenceException(nameof(_databaseRepo));

            var request = JsonConvert.DeserializeObject<ToDoItemAction>(message);

            return request != null ? _actionHandlers[request.RequestAction](request) : string.Empty;
        }

        private string AddPerson(ToDoItemAction request)
        {
            _databaseRepo?.AddPerson(request.FirstName, request.LastName);
            var person = _databaseRepo.GetPersonByName(request.FirstName, request.LastName);

            return person != null ? JsonConvert.SerializeObject(person) : string.Empty;
        }

        private string AddToDoItem(ToDoItemAction request)
        {
            return _databaseRepo.AddToDoItem(request.PersonId, request.ToDoItem) ? "Success" : "Failed";
        }

        private string UpdateToDoItem(ToDoItemAction request)
        {

            return string.Empty;
        }

        private string ViewAllItems(ToDoItemAction request)
        {

            return string.Empty;
        }

        private string ViewPersons(ToDoItemAction request)
        {
            var items = _databaseRepo.GetAllPersons();

            var jsonResponse = JsonConvert.SerializeObject(items, Formatting.Indented);

            return jsonResponse;
        }

        private string DeleteUser(ToDoItemAction request)
        {

            return string.Empty;
        }

        private string DeleteUserAllItems(ToDoItemAction request)
        {

            return string.Empty;
        }

        private string GetPerson(ToDoItemAction request)
        {
            var person = _databaseRepo.GetPersonByName(request.FirstName, request.LastName);
            var jsonPerson = JsonConvert.SerializeObject(person, Formatting.Indented);
            return jsonPerson;
        }
        #endregion
    }
}
