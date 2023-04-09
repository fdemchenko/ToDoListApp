using TodoListAppMVC.DAL.Models;
using TodoListAppMVC.DTO;

namespace TodoListAppMVC.Services;
public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task AddAsync(IncomingCategoryDTO newCategoryDTO);
    Task DeleteByIdAsync(int id);
}