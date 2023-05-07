using TodoListDAL.Models;

namespace TodoListDAL.Repositories;

public interface ITodoItemRepository
{
    IEnumerable<TodoItem> GetAll();
    TodoItem? GetById(int id);
    TodoItem? DeleteById(int id);
    TodoItem? Update(TodoItem todoItem);
    TodoItem Add(TodoItem todoItem);
    IEnumerable<TodoItem> GetByCategoryId(int categoryId);
}