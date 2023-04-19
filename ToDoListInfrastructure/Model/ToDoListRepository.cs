namespace ToDoListInfrastructure.Model
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ToDoListDbContext _toDoListDbContext;

        #region ctor

        public ToDoListRepository(ToDoListDbContext dbContext) 
        {
            _toDoListDbContext = dbContext;
        }
        #endregion

        public List<Person> GetAllPersons()
        {
            return _toDoListDbContext.Persons.Select(p => p).ToList();
        }
    }
}
