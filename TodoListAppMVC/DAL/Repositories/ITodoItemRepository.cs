using TodoListAppMVC.DAL.Models;

namespace TodoListAppMVC.DAL.Repositories;

public interface ITodoItemRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<TodoItem?> GetByIdAsync(int id);
    Task DeleteByIdAsync(int id);
    Task UpdateAsync(TodoItem todoItem);
    Task AddAsync(TodoItem todoItem);
}