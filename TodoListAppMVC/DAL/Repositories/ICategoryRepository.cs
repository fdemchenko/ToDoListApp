using TodoListAppMVC.DAL.Models;

namespace TodoListAppMVC.DAL.Repositories;

public interface ICategoryRepository
{
    IEnumerable<Category> GetAll();
    Category? GetByName(string name);
    Category? GetById(int id);
    void DeleteById(int id);
    void Add(Category todoItem);
}