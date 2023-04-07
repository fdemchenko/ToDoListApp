using TodoListAppMVC.DAL.Models;

namespace TodoListAppMVC.DAL.Repositories;

public interface ITodoItemRepository
{
    Task<IEnumerable<TodoItem>> GetAll();
    Task<TodoItem?> GetById(int id);
    Task DeleteById(int id);
    Task Update(TodoItem todoItem);
    Task Add(TodoItem todoItem);
}