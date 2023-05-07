using TodoListAppMVC.DTO;
using TodoListDAL.Models;

namespace TodoListAppMVC.Models;

public class CategoryCreationViewModel
{
    public IncomingCategoryDTO NewCategory { get; set; } = null!;

    public IEnumerable<Category> Categories { get; set; } = null!;
}