using ToDoListInfrastructure.Model;

namespace ToDoListWebApp.Interfaces
{
    public interface IToDoListManager
    {
        int CancellationTimeout { get; set; }

        Task<List<Person>> GetAllPersonsAsync();

        Task AddPersonAsync(string firstName, string lastName);

        Task DeletePersonAsync(int userId);

        Task AddToDoItemAsync(string firstName, string lastName, ToDoItemDescriptor toDoItem);

        Task<List<ToDoItem>> GetUserToDoItemsAsync(int userId);

        Task DeleteToDoItemAsync(int userId, int itemId);

        Task DeletePersonToDoItemsAsync(int userId);
    }
}
