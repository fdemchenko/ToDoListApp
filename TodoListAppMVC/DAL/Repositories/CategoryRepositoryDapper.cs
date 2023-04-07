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

    public async Task Add(Category category)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("INSERT INTO Categories (Name) VALUES (@Name)", category);
    }

    public async Task DeleteById(int id)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM Categories WHERE Id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Category>("SELECT * FROM Categories"); 
    }

    public async Task<Category?> GetByName(string name)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Category>(
            "SELECT * FROM Categories WHERE Name = @name", new { Name = name }); 
    }
}