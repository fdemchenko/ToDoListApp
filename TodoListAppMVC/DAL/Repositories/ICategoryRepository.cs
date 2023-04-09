using TodoListAppMVC.DAL.Models;

namespace TodoListAppMVC.DAL.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByNameAsync(string name);
    Task DeleteByIdAsync(int id);
    Task AddAsync(Category todoItem);
}