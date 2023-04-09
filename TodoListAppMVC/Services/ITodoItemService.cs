using TodoListAppMVC.DAL.Models;
using TodoListAppMVC.DTO;

namespace TodoListAppMVC.Services;

public interface ITodoItemService
{
    Task<IEnumerable<TodoItem>> GetAllSortedAsync();
    Task AddAsync(IncomingTodoItemDTO newTodoItem);
    Task DeleteAsync(int id);
    Task SetCompletedAsync(int id);
    Task<TodoItem?> GetByIdAsync(int id);
    Task UpdateAsync(UpdateTodoItemDTO updatedTodoItem);
}