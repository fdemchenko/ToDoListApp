using TodoListAppMVC.DAL.Models;
using TodoListAppMVC.DAL.Repositories;
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

    public async Task Add(IncomingCategoryDTO newCategoryDTO)
    {
        Category? existingCategory = await _repository.GetByName(newCategoryDTO.Name!);
        if (existingCategory is not null)
            return;
        
        await _repository.Add(_mapper.Map<Category>(newCategoryDTO));
    }

    public async Task DeleteById(int id)
    {
        await _repository.DeleteById(id);
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _repository.GetAll();
    }
}