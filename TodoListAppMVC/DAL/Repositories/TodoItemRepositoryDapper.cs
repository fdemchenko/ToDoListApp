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

    public async Task Add(TodoItem todoItem)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(@"INSERT INTO TodoItems (Name, DueDate, Completed, CategoryId) VALUES
                                    (@Name, @DueDate, @Completed, @CategoryId)", todoItem);
    }

    public async Task DeleteById(int id)
    { 
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM TodoItems WHERE Id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<TodoItem>> GetAll()
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<TodoItem, Category, TodoItem>(
            @"SELECT tdi.Id, tdi.Name, tdi.DueDate, tdi.Completed, ctg.Id, ctg.Name
            FROM TodoItems tdi JOIN Categories ctg ON ctg.Id = tdi.CategoryId",
            (todoItem, category) => {
                todoItem.CategoryId = category.Id;
                todoItem.Category = category;
                return todoItem;
            }); 
    }

    public async Task<TodoItem?> GetById(int id)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        IEnumerable<TodoItem> todoItems =  await connection.QueryAsync<TodoItem, Category, TodoItem>(
            @"SELECT tdi.Id, tdi.Name, tdi.DueDate, tdi.Completed, ctg.Id, ctg.Name 
            FROM TodoItems tdi JOIN Categories ctg ON ctg.Id = tdi.CategoryId WHERE tdi.Id = @Id",
            (todoItem, category) => {
                todoItem.CategoryId = category.Id;
                todoItem.Category = category;
                return todoItem;
            }, new { Id = id }); 
        return todoItems.FirstOrDefault();
    }

    public async Task Update(TodoItem todoItem)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(@"UPDATE TodoItems SET Name = @Name, DueDate = @DueDate, Completed = @Completed, CategoryId = @CategoryId WHERE Id = @Id", todoItem);
    }
}