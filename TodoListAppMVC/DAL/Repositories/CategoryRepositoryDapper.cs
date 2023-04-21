using TodoListAppMVC.DAL.Models;
using Dapper;
using Npgsql;

namespace TodoListAppMVC.DAL.Repositories;

public class CategoryRepositoryDapper : ICategoryRepository
{
    private readonly string _connectionString;
    public CategoryRepositoryDapper(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Default");
    }

    public void Add(Category category)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        connection.Execute("INSERT INTO Categories (Name) VALUES (@Name)", category);
    }

    public void DeleteById(int id)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        connection.Execute("DELETE FROM Categories WHERE Id = @Id", new { Id = id });
    }

    public IEnumerable<Category> GetAll()
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        return connection.Query<Category>("SELECT * FROM Categories"); 
    }

    public Category? GetByName(string name)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        return connection.QueryFirstOrDefault<Category>(
            "SELECT * FROM Categories WHERE Name = @name", new { Name = name }); 
    }

    public Category? GetById(int id)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        return connection.QueryFirstOrDefault<Category>(
            "SELECT * FROM Categories WHERE Id = @Id", new { Id = id }); 
    }
}