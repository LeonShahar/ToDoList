using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListInfrastructure.Model
{
    [Table("Person")]
    public sealed class Person
    {
        #region public properties

        public int ID { get; set; }     

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public List<ToDoItem>   ToDoItems { get; set; } = new List<ToDoItem>();

        #endregion
    }
}
