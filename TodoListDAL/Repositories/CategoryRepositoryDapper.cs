using Dapper;
using Npgsql;
using TodoListDAL.Models;
using TodoListDAL.Exceptions;
using Microsoft.Extensions.Configuration;

namespace TodoListDAL.Repositories;

public class CategoryRepositoryDapper : ICategoryRepository
{
    private readonly string _connectionString;
    public CategoryRepositoryDapper(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Default") ??
            throw new InvalidConfigurationException("Cannot get default connection string");
    }

    public Category Add(Category category)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        int newId = connection.ExecuteScalar<int>(@"INSERT INTO Categories (Name) 
            VALUES (@Name) RETURNING Id", category);
        category.Id = newId;
        return category;
    }

    public Category? DeleteById(int id)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        connection.Execute("DELETE FROM Categories WHERE Id = @Id", new { Id = id });
        return GetById(id);
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