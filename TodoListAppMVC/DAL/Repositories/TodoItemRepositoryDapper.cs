using TodoListAppMVC.DAL.Models;
using Dapper;
using Npgsql;

namespace TodoListAppMVC.DAL.Repositories;

public class TodoItemRepositoryDapper : ITodoItemRepository
{
    private readonly string _connectionString;
    public TodoItemRepositoryDapper(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Default");
    }

    public void Add(TodoItem todoItem)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        connection.Execute(@"INSERT INTO TodoItems (Name, DueDate, Completed, CategoryId) VALUES
                                    (@Name, @DueDate, @Completed, @CategoryId)", todoItem);
    }

    public void DeleteById(int id)
    { 
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        connection.Execute("DELETE FROM TodoItems WHERE Id = @Id", new { Id = id });
    }

    public IEnumerable<TodoItem> GetAll()
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        return connection.Query<TodoItem, Category, TodoItem>(
            @"SELECT tdi.Id, tdi.Name, tdi.DueDate, tdi.Completed, ctg.Id, ctg.Name
            FROM TodoItems tdi JOIN Categories ctg ON ctg.Id = tdi.CategoryId",
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

    public void Update(TodoItem todoItem)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        connection.Execute(@"UPDATE TodoItems SET Name = @Name, DueDate = @DueDate, Completed = @Completed, CategoryId = @CategoryId WHERE Id = @Id", todoItem);
    }
}