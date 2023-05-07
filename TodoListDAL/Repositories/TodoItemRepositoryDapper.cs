using TodoListDAL.Models;
using TodoListDAL.Exceptions;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace TodoListDAL.Repositories;

public class TodoItemRepositoryDapper : ITodoItemRepository
{
    private readonly string _connectionString;
    public TodoItemRepositoryDapper(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Default") ?? 
            throw new InvalidConfigurationException("Cannot get default connection string");
    }

    public TodoItem Add(TodoItem todoItem)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        int newId = connection.Execute(@"INSERT INTO TodoItems (Name, DueDate, Completed, CategoryId) VALUES
                                    (@Name, @DueDate, @Completed, @CategoryId) RETURNING Id", todoItem);
        todoItem.Category = connection.QueryFirstOrDefault<Category>("SELECT * FROM Categories WHERE Id = @Id", new { Id = todoItem.CategoryId });
        todoItem.Id = newId;
        return todoItem;
    }

    public TodoItem? DeleteById(int id)
    {
        TodoItem? todoItemToDelete = GetById(id);
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        connection.Execute("DELETE FROM TodoItems WHERE Id = @Id", new { Id = id });
        return todoItemToDelete;
    }

    public IEnumerable<TodoItem> GetAll()
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        return connection.Query<TodoItem, Category, TodoItem>(
            @"SELECT tdi.Id, tdi.Name, tdi.DueDate, tdi.Completed, ctg.Id, ctg.Name
            FROM TodoItems tdi JOIN Categories ctg ON ctg.Id = tdi.CategoryId ORDER BY tdi.Completed, tdi.DueDate",
            (todoItem, category) => {
                todoItem.CategoryId = category.Id;
                todoItem.Category = category;
                return todoItem;
            }); 
    }

    public TodoItem? GetById(int id)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        IEnumerable<TodoItem> todoItems =  connection.Query<TodoItem, Category, TodoItem>(
            @"SELECT tdi.Id, tdi.Name, tdi.DueDate, tdi.Completed, ctg.Id, ctg.Name 
            FROM TodoItems tdi JOIN Categories ctg ON ctg.Id = tdi.CategoryId WHERE tdi.Id = @Id",
            (todoItem, category) => {
                todoItem.CategoryId = category.Id;
                todoItem.Category = category;
                return todoItem;
            }, new { Id = id }); 
        return todoItems.FirstOrDefault();
    }

    public IEnumerable<TodoItem> GetByCategoryId(int categoryId)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        IEnumerable<TodoItem> todoItems =  connection.Query<TodoItem, Category, TodoItem>(
            @"SELECT tdi.Id, tdi.Name, tdi.DueDate, tdi.Completed, ctg.Id, ctg.Name 
            FROM TodoItems tdi JOIN Categories ctg ON ctg.Id = tdi.CategoryId WHERE tdi.CategoryId = @CategoryId",
            (todoItem, category) => {
                todoItem.CategoryId = category.Id;
                todoItem.Category = category;
                return todoItem;
            }, new { CategoryId = categoryId }); 
        return todoItems;
    }

    public TodoItem? Update(TodoItem todoItem)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        connection.Execute(@"UPDATE TodoItems SET 
            Name = @Name, DueDate = @DueDate, Completed = @Completed, 
            CategoryId = @CategoryId WHERE Id = @Id", todoItem);
        
        return GetById(todoItem.Id);
    }
}