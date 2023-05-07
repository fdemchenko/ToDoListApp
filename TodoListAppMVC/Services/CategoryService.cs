using TodoListDAL.Repositories;
using TodoListDAL.Models;
using TodoListAppMVC.DTO;
using AutoMapper;

namespace TodoListAppMVC.Services;
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public void Add(IncomingCategoryDTO newCategoryDTO)
    {
        Category? existingCategory = _repository.GetByName(newCategoryDTO.Name!);
        if (existingCategory is not null)
            return;
        
        _repository.Add(_mapper.Map<Category>(newCategoryDTO));
    }

    public void DeleteById(int id)
    {
        _repository.DeleteById(id);
    }

    public IEnumerable<Category> GetAll()
    {
        return _repository.GetAll();
    }
}