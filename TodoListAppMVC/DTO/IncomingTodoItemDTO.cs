using System.ComponentModel.DataAnnotations;

namespace TodoListAppMVC.DTO;

public class IncomingTodoItemDTO
{
    [Required(ErrorMessage = "Todo item name is required field")]
    [StringLength(100, ErrorMessage = "Todo item name cannot be longest than 50 characters")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Due date is required field")]
    public DateTime DueDate { get; set; }
    public bool Completed { get; set; }
    [Required(ErrorMessage = "Todo item's category is required field")]
    public int CategoryId { get; set; }
}