using ToDoListInfrastructure.Model;
using ToDoListRepo.Interfaces;

namespace ToDoListRepo.Repositories
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ToDoListDbContext _toDoListDbContext;
        private readonly object _dbContextLock = new object();

        #region ctor

        public ToDoListRepository(ToDoListDbContext dbContext)
        {
            _toDoListDbContext = dbContext;
        }
        #endregion

        public List<Person> GetAllPersons()
        {
            lock (_dbContextLock)
            {
                return _toDoListDbContext.Persons.Select(p => p).ToList();
            }
        }

        public Person? AddPerson(string? firstName, string? lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                throw new ArgumentNullException($"{nameof(firstName)}-{lastName}");

            var person = _toDoListDbContext.Persons.FirstOrDefault(p => p.FirstName == firstName && p.LastName == lastName);
            if (person == null)
            {
                lock (_dbContextLock)
                {
                    _toDoListDbContext.Persons.Add(new Person { FirstName = firstName, LastName = lastName });
                    _toDoListDbContext.SaveChanges();

                    person = _toDoListDbContext.Persons.FirstOrDefault(p => p.FirstName == firstName && p.LastName == lastName);
                }
            }

            return person;
        }

        public bool DeleteUser(int personId)
        {
            var person = GetPersonById(personId);
            if (person == null)
            {
                return false;
            }

            lock (_dbContextLock)
            {
                _toDoListDbContext.Persons.Remove(person);
                _toDoListDbContext.SaveChanges();
                return true;
            }
        }

        public bool AddToDoItem(int personId, ToDoItemDescriptor? toDoItem)
        {
            if (toDoItem == null)
                throw new ArgumentNullException(nameof(toDoItem));

            lock (_dbContextLock)
            {
                var item = new ToDoItem(toDoItem, personId);
                _toDoListDbContext.ToDoItems.Add(item);
                _toDoListDbContext.SaveChanges();
                return true;
            }
        }

        public List<ToDoItem> GetUserToDoItems(int personId)
        {
            var items = new List<ToDoItem>();
            try
            {
                lock (_dbContextLock)
                {
                    var person = _toDoListDbContext.Persons.Find(true, personId);
                    if (person != null)
                    {
                        items.AddRange(_toDoListDbContext.ToDoItems.Where<ToDoItem>(i => i.PersonID == personId));
                    }
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
            lock (_dbContextLock)
            {
                var toDoItem = _toDoListDbContext.ToDoItems.FirstOrDefault(i => i.ID == itemId && i.PersonID == personId);
                if (toDoItem != null)
                {
                    _toDoListDbContext.Remove(toDoItem.ID);
                    _toDoListDbContext.SaveChanges();
                }
                return true;
            }
        }

        public bool DeleteUserToDoItemsbool(int personId)
        {
            lock (_dbContextLock)
            {
                var items = _toDoListDbContext.ToDoItems.Where(i => i.PersonID == personId);
                if (items != null)
                {
                    _toDoListDbContext.ToDoItems.RemoveRange(items);
                    _toDoListDbContext.SaveChanges();
                }

                return true;
            }
        }

        public Person? GetPersonByName(string? firstName, string? lastName)
        {
            lock (_dbContextLock)
            {
                return _toDoListDbContext.Persons.FirstOrDefault(p => p.FirstName == firstName && p.LastName == lastName);
            }
        }

        public Person? GetPersonById(int personId)
        {
            lock (_dbContextLock)
            {
                return _toDoListDbContext.Persons.Find(true, personId);
            }
        }
    }
}
