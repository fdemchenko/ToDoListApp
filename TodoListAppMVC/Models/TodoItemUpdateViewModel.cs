using TodoListAppMVC.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TodoListAppMVC.Models;

public class TodoItemUpdateViewModel
{
    public UpdateTodoItemDTO TodoItemToUpdate { get; set; } = null!;
    public IEnumerable<SelectListItem> Categories { get; set; } = null!;
}