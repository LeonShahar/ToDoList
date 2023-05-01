using ToDoListInfrastructure.Model;

namespace ToDoListInfrastructure.Messaging
{
    public class ToDoItemAction
    {
        #region public properties

        public int PersonId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public ToDoItemDescriptor? ToDoItem { get; set; }

        public int ToDoItemId { get; set; }

        public ToDoItemActionEnum RequestAction { get; set; }

        #endregion
    }
}
