using TodoListAppMVC.DTO;
using TodoListAppMVC.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace TodoListAppMVC.Models;

public class TodoItemCreationViewModel
{
    public IncomingTodoItemDTO NewTodoItem { get; set; } = null!;

    public IEnumerable<TodoItem> TodoItems { get; set; } = null!;
    public IEnumerable<SelectListItem> Categories { get; set; } = null!;
}