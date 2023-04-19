using Microsoft.AspNetCore.Mvc;
using ToDoListInfrastructure.Model;
using ToDoListWebApp.Interfaces;

namespace ToDoListWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController
    {
        #region private members

        private IToDoListManager _toDoListManager;

        #endregion

        #region ctor

        public ToDoListController(IToDoListManager manager)
        {
            _toDoListManager = manager;
        }
        #endregion

        #region REST Api

        [HttpGet]
        public async Task<ActionResult<List<Person>>> GetAllPersons() 
        {
            var persons = await _toDoListManager.GetAllPersonsAsync();

            return persons;
        }

        #endregion
    }
}
