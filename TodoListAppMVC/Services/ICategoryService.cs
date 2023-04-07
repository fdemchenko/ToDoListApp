using TodoListAppMVC.DAL.Models;
using TodoListAppMVC.DTO;

namespace TodoListAppMVC.Services;
public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAll();
    Task Add(IncomingCategoryDTO newCategoryDTO);
    Task DeleteById(int id);
}