using System.ComponentModel.DataAnnotations;

namespace TodoListAppMVC.DTO;

public class IncomingCategoryDTO
{
    [Required]
    [StringLength(50)]
    public string? Name { get; set; }
}