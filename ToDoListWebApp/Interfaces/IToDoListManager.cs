using ToDoListInfrastructure.Model;

namespace ToDoListWebApp.Interfaces
{
    public interface IToDoListManager
    {
        Task<List<Person>> GetAllPersonsAsync();
    }
}
