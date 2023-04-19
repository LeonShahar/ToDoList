using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListInfrastructure.Model
{
    [Table("ToDoItem")]
    [Owned]
    public sealed class ToDoItem
    {
        #region public properties
        public int ID { get; set; }

        [ForeignKey("PersonID")]
        public int PersonID { get; set; }

        public PriorityEnum Priority { get; set; } = PriorityEnum.None;

        public StatusEnum Status { get; set; } = StatusEnum.None;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        #endregion
    }
}
