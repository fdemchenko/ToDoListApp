using System.ComponentModel.DataAnnotations;

namespace TodoListAppMVC.DTO;
public class UpdateTodoItemDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string? Name { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
    public bool Completed { get; set; }
    [Required]
    public int CategoryId { get; set; }
}