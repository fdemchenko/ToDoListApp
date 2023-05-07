using TodoListDAL.Models;

namespace TodoListDAL.Repositories;

public interface ICategoryRepository
{
    IEnumerable<Category> GetAll();
    Category? GetByName(string name);
    Category? GetById(int id);
    Category? DeleteById(int id);
    Category Add(Category category);
}