namespace ToDoListInfrastructure.Model
{
    public  interface IToDoListRepository
    {
        List<Person> GetAllPersons();
    }
}
