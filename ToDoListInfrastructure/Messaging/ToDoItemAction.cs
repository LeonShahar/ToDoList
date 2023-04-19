namespace ToDoListInfrastructure.Messaging
{
    public class ToDoItemAction
    {
        #region public properties

        public int PersonId { get; set; }

        public ToDoItemActionEnum RequestAction { get; set; }

        #endregion
    }
}
