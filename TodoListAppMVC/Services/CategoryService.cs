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

    public async Task AddAsync(IncomingCategoryDTO newCategoryDTO)
    {
        Category? existingCategory = await _repository.GetByNameAsync(newCategoryDTO.Name!);
        if (existingCategory is not null)
            return;
        
        await _repository.AddAsync(_mapper.Map<Category>(newCategoryDTO));
    }

    public async Task DeleteByIdAsync(int id)
    {
        await _repository.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
}