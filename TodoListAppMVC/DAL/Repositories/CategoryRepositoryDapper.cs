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

    public async Task AddAsync(Category category)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("INSERT INTO Categories (Name) VALUES (@Name)", category);
    }

    public async Task DeleteByIdAsync(int id)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM Categories WHERE Id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Category>("SELECT * FROM Categories"); 
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Category>(
            "SELECT * FROM Categories WHERE Name = @name", new { Name = name }); 
    }
}