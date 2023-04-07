using TodoListAppMVC.DAL.Models;
using TodoListAppMVC.DTO;

namespace TodoListAppMVC.Services;

public interface ITodoItemService
{
    Task<IEnumerable<TodoItem>> GetAllSorted();
    Task Add(IncomingTodoItemDTO newTodoItem);
    Task Delete(int id);
    Task SetCompleted(int id);
    Task<TodoItem?> GetById(int id);
    Task Update(UpdateTodoItemDTO updatedTodoItem);
}