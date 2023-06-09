﻿using ToDoListInfrastructure.Model;

namespace ToDoListRepo.Interfaces
{
    public interface IToDoListRepository
    {
        List<Person> GetAllPersons();

        Person? AddPerson(string? firatName, string? lastName);

        bool DeleteUser(int userId);

        bool AddToDoItem(int userId, ToDoItemDescriptor? toDoItem);

        List<ToDoItem> GetUserToDoItems(int userId);

        bool DeleteToDoItem(int userId, int itemId);

        bool DeleteUserToDoItemsbool(int userId);

        Person? GetPersonByName(string? firstName, string? lastName);

        Person? GetPersonById(int userId);
    }
}
