namespace ToDoListInfrastructure.Model
{
    public class ToDoItemDescriptor
    {
        #region public properties

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public PriorityEnum Priority { get; set; } = PriorityEnum.None;

        public StatusEnum Status { get; set; } = StatusEnum.None;

        #endregion

        #region ctor

        public ToDoItemDescriptor() { }

        public ToDoItemDescriptor(ToDoItemDescriptor source) 
        {
            Name = source.Name;
            Description = source.Description;
            Priority = source.Priority;
            Status = source.Status;
        }
        #endregion
    }
}
