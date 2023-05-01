using Newtonsoft.Json;
using ToDoListInfrastructure.Messaging;
using ToDoListInfrastructure.Model;
using ToDoListWebApp.Interfaces;

namespace ToDoListWebApp.Managers
{
    public class ToDoListManager : IToDoListManager
    {
        #region private members

        private RabbitPublisher _pblisher;

        #endregion

        #region publilc properties

        public int CancellationTimeout { get; set; }

        #endregion

        #region ctor

        public ToDoListManager(IHostApplicationLifetime hostAppLifetime, RabbitPublisher publisher)
        {
            _pblisher = publisher;
            hostAppLifetime.ApplicationStopping.Register(_pblisher.Dispose);

            CancellationTimeout = 1000;
        }

        #endregion

        #region IToDoListManager implementation
        
        public async Task<List<Person>?> GetAllPersonsAsync()
        {
            var request = new ToDoItemAction
            {
                PersonId = -1,
                RequestAction = ToDoItemActionEnum.ViewPersons
            };

            var allPersons = await SendRequestAsync<List<Person>?>(request, new CancellationTokenSource(CancellationTimeout).Token);

            return allPersons;
        }

        public async Task AddPersonAsync(string firstName, string lastName)
        {
            var request = new ToDoItemAction
            {
                PersonId = -1,
                FirstName = firstName,
                LastName = lastName,
                RequestAction = ToDoItemActionEnum.AddPerson
            };

            await SendRequestAsync(request, new CancellationTokenSource(CancellationTimeout).Token);
        }

        public async Task DeletePersonAsync(int personId)
        {
            var request = new ToDoItemAction
            {
                PersonId = personId,
                RequestAction = ToDoItemActionEnum.DeletePerson
            };

            await SendRequestAsync(request, new CancellationTokenSource(CancellationTimeout).Token);
        }

        public async Task AddToDoItemAsync(string firstName, string lastName, ToDoItemDescriptor toDoItem)
        {
            var getPersonRequest = new ToDoItemAction
            {
                FirstName = firstName,
                LastName = lastName,
                RequestAction = ToDoItemActionEnum.GetPerson
            };
            var person = await SendRequestAsync<Person>(getPersonRequest, new CancellationTokenSource(CancellationTimeout).Token);

            if (person != null)
            {
                var request = new ToDoItemAction
                {
                    PersonId = person.ID,
                    ToDoItem = toDoItem,
                    RequestAction = ToDoItemActionEnum.AddToDoItem
                };
                await SendRequestAsync(request, new CancellationTokenSource(CancellationTimeout).Token);
            }
        }

        public async Task<List<ToDoItem>?> GetUserToDoItemsAsync(int personId)
        {
            var request = new ToDoItemAction
            {
                PersonId = personId,
                RequestAction = ToDoItemActionEnum.ViewAllItems
            };

            var items = await SendRequestAsync<List<ToDoItem>?>(request, new CancellationTokenSource(CancellationTimeout).Token);

            return items;
        }

        public async Task DeleteToDoItemAsync(int personId, int itemId)
        {
            var request = new ToDoItemAction
            {
                PersonId = personId,
                ToDoItemId = itemId,
                RequestAction = ToDoItemActionEnum.DeleteToDoItem
            };

            await SendRequestAsync(request, new CancellationTokenSource(CancellationTimeout).Token);
        }

        public async Task DeletePersonToDoItemsAsync(int personId)
        {
            var request = new ToDoItemAction
            {
                PersonId = personId,
                RequestAction = ToDoItemActionEnum.DeletePersonAllItems
            };

            await SendRequestAsync(request, new CancellationTokenSource(CancellationTimeout).Token);
        }

        #endregion

        #region private methods

        private async Task<T?> SendRequestAsync<T>(ToDoItemAction request, CancellationToken cancellationToken = default)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _pblisher.SendMessageAsync(jsonRequest, cancellationToken);

            var result = JsonConvert.DeserializeObject<T>(response);

            return result;
        }

        private async Task SendRequestAsync(ToDoItemAction request, CancellationToken cancellationToken = default)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _pblisher.SendMessageAsync(jsonRequest, cancellationToken);

        }
        #endregion
    }
}
