using TodoListDAL.Models;
using TodoListAppMVC.DTO;

namespace TodoListAppMVC.Services;

public interface ITodoItemService
{
    IEnumerable<TodoItem> GetAllSorted();
    void Add(IncomingTodoItemDTO newTodoItem);
    void Delete(int id);
    void SetCompleted(int id);
    TodoItem? GetById(int id);
    void Update(UpdateTodoItemDTO updatedTodoItem);
}