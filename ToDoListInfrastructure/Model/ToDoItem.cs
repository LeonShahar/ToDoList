using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListInfrastructure.Model
{
    [Table("ToDoItem")]
    [Owned]
    public sealed class ToDoItem : ToDoItemDescriptor
    {
        #region public properties
        public int ID { get; set; }

        [ForeignKey("PersonID")]
        public int PersonID { get; set; }

        #endregion

        #region ctor

        public ToDoItem() : base()
        {

        }

        public ToDoItem(ToDoItemDescriptor itemDescriptor, int personID) : base(itemDescriptor)
        {
            PersonID = personID;
        }
        #endregion
    }
}
