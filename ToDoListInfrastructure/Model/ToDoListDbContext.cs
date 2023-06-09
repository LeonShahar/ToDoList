﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ToDoListInfrastructure.Model
{
    public sealed class ToDoListDbContext : DbContext
    {
        private readonly IConfiguration? _configuration;


        private static int _personId = 0;
        private static int _itemId = 0;

        private ModelBuilder? _modelBuilder;
        #region ctor

        public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : base(options)
        {
            
        }
        #endregion

        #region collections

        public DbSet<Person> Persons { get; set; }

        public DbSet<ToDoItem> ToDoItems { get; set; }

        #endregion


        #region protected methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ToDoListItems");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().Property(p => p.FirstName).IsRequired();
            modelBuilder.Entity<Person>().Property(p => p.FirstName).IsRequired();
            modelBuilder.Entity<Person>().Property(p => p.LastName).IsRequired();
            modelBuilder.Entity<Person>().Property(p => p.LastName).IsRequired();
            modelBuilder.Entity<ToDoItem>().Property(i => i.Name).IsRequired();
            modelBuilder.Entity<ToDoItem>().Property(i => i.Priority).IsRequired();
            modelBuilder.Entity<ToDoItem>().Property(i => i.Status).IsRequired();

            _modelBuilder = modelBuilder;
        }

        #endregion
    }
}
