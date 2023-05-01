using Microsoft.AspNetCore.Mvc;
using ToDoListInfrastructure.Model;
using ToDoListWebApp.Interfaces;

namespace ToDoListWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListController
    {
        #region private members

        private IToDoListManager _toDoListManager;

        #endregion

        #region ctor

        public ToDoListController(IToDoListManager manager)
        {
            _toDoListManager = manager;
            _toDoListManager.CancellationTimeout = 1000;
        }
        #endregion

        #region REST Api

        [Route("/GetAllPersons")]
        [HttpGet]
        public async Task<ActionResult<List<Person>>> GetAllPersons() 
        {
            var persons = await _toDoListManager.GetAllPersonsAsync();

            return persons;
        }

        [Route("/GetUserToDoItems")]
        [HttpGet]
        public async Task<List<ToDoItem>> GetUserToDoItems(int personId)
        {
            var toDoItems = await _toDoListManager.GetUserToDoItemsAsync(personId);
            return toDoItems;
        }

        [Route("/AddUser")]
        [HttpPost]
        public async Task AddUser(string firstName, string lastName)
        {
            await _toDoListManager.AddPersonAsync(firstName, lastName);
        }

        [Route("/AddToDoItem")]
        [HttpPost]
        public async Task AddToDoItem(string firstName, string lastName, ToDoItemDescriptor toDoItem)
        {
            await _toDoListManager.AddToDoItemAsync(firstName, lastName, toDoItem);
        }

        [Route("/DeletePerson")]
        [HttpDelete]
        public async Task DeleteUser(int personId)
        {
            await _toDoListManager.DeletePersonAsync(personId);
        }
        [Route("/DeleteToDoItem")]
        [HttpDelete]
        public async Task DeleteToDoItem(int personId, int itemId)
        {
            await _toDoListManager.DeleteToDoItemAsync(personId, itemId);
        }

        [Route("/DeleteUserToDoItems")]
        [HttpDelete]
        public async Task DeleteUserToDoItems(int personId)
        {
            await _toDoListManager.DeletePersonToDoItemsAsync(personId);
        }

        #endregion
    }
}
