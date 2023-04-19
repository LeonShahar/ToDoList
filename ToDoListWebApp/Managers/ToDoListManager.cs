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

        #region ctor

        public ToDoListManager(RabbitPublisher publisher)
        {
            _pblisher = publisher;
        }

        #endregion

        #region IToDoListManager implementation

        public async Task<List<Person>> GetAllPersonsAsync()
        {
            var reuest = new ToDoItemAction
            {
                PersonId = -1,
                RequestAction = ToDoItemActionEnum.ViewAllItems
            };
            var jsonReuest = JsonConvert.SerializeObject(reuest);
            var response = await _pblisher.SendMessageAsync(jsonReuest);

            var allPersons = JsonConvert.DeserializeObject<List<Person>>(response);

            return allPersons;
        }
        #endregion
    }
}
