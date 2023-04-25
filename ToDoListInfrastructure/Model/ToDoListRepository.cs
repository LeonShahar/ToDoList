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

        public bool AddPerson(string firstName, string lastName)
        {
            var person = GetPersonByName(firstName, lastName);
            if (person == null)
            {
                _toDoListDbContext.Persons.Add(new Person { FirstName = firstName, LastName = lastName });
                _toDoListDbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool DeleteUser(int personId)
        {

            var person = GetPersonById(personId);
            if (person == null)
            {
                return false;
            }

            _toDoListDbContext.Persons.Remove(person);
            _toDoListDbContext.SaveChanges();
            return true;
        }

        public bool AddToDoItem(int personId, ToDoItemDescriptor toDoItem)
        {
            var item = new ToDoItem(toDoItem, personId);
            _toDoListDbContext.ToDoItems.Add(item);
            _toDoListDbContext.SaveChanges();
            return true;
        }

        public List<ToDoItem> GetUserToDoItems(int personId)
        {
            var items = new List<ToDoItem>();
            try
            {
                var person = _toDoListDbContext.Persons.Find(true, personId);
                if (person != null)
                {
                    items.AddRange(_toDoListDbContext.ToDoItems.Where<ToDoItem>(i => i.PersonID == personId));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return items;
        }

        public bool DeleteToDoItem(int personId, int itemId)
        {
            var toDoItem = _toDoListDbContext.ToDoItems.FirstOrDefault(i => i.ID == itemId && i.PersonID == personId);
            if (toDoItem != null)
            {
                _toDoListDbContext.Remove(toDoItem.ID);
                _toDoListDbContext.SaveChanges();
            }
            return true;
        }

        public bool DeleteUserToDoItemsbool(int personId)
        {
            var items = _toDoListDbContext.ToDoItems.Where(i => i.PersonID == personId);
            if (items != null)
            {
                _toDoListDbContext.ToDoItems.RemoveRange(items);
                _toDoListDbContext.SaveChanges();
            }

            return true;
        }

        public Person? GetPersonByName(string firstName, string lastName)
        {
            return _toDoListDbContext.Persons.FirstOrDefault(p => p.FirstName == firstName && p.LastName == lastName);
        }

        public Person? GetPersonById(int personId)
        {
            return _toDoListDbContext.Persons.Find(true, personId);
        }


    }
}
