using TodoListAppMVC.DAL.Models;

namespace TodoListAppMVC.DAL.Repositories;

public interface ITodoItemRepository
{
    IEnumerable<TodoItem> GetAll();
    TodoItem? GetById(int id);
    void DeleteById(int id);
    void Update(TodoItem todoItem);
    void Add(TodoItem todoItem);
    IEnumerable<TodoItem> GetByCategoryId(int categoryId);
}