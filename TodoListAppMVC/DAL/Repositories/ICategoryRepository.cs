using TodoListAppMVC.DAL.Models;

namespace TodoListAppMVC.DAL.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAll();
    Task<Category?> GetByName(string name);
    Task DeleteById(int id);
    Task Add(Category todoItem);
}