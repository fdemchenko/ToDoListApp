using System.ComponentModel.DataAnnotations;

namespace TodoListAppMVC.DTO;

public class IncomingCategoryDTO
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, ErrorMessage = "Category name cannot be longest than 50 characters")]
    public string? Name { get; set; }
}