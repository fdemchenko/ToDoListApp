using TodoListAppMVC.DAL.Models;
using TodoListAppMVC.DTO;

namespace TodoListAppMVC.Services;
public interface ICategoryService
{
    IEnumerable<Category> GetAll();
    void Add(IncomingCategoryDTO newCategoryDTO);
    void DeleteById(int id);
}