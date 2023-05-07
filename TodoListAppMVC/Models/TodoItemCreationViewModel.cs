using TodoListAppMVC.DTO;
using TodoListDAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace TodoListAppMVC.Models;

public class TodoItemCreationViewModel
{
    public IncomingTodoItemDTO NewTodoItem { get; set; } = null!;

    public IEnumerable<TodoItem> TodoItems { get; set; } = null!;
    public IEnumerable<SelectListItem> Categories { get; set; } = null!;
}