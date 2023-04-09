using TodoListAppMVC.DAL.Models;
using TodoListAppMVC.DTO;
using TodoListAppMVC.DAL.Repositories;
using AutoMapper;

namespace TodoListAppMVC.Services;

public class TodoItemService : ITodoItemService
{
    private readonly ITodoItemRepository _repository;
    private readonly IMapper _mapper;
    public TodoItemService(ITodoItemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task AddAsync(IncomingTodoItemDTO newTodoItemDTO)
    {
        TodoItem newItem = _mapper.Map<TodoItem>(newTodoItemDTO);
        await _repository.AddAsync(newItem);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable<TodoItem>> GetAllSortedAsync()
    {
        IEnumerable<TodoItem> items = await _repository.GetAllAsync();
        return items.OrderBy(item => item, new TodoItemComparer());
    }

    public Task<TodoItem?> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public async Task SetCompletedAsync(int id)
    {
        TodoItem? todoItem = await _repository.GetByIdAsync(id);
        if (todoItem is not null) 
        {
            todoItem.Completed = true;
            await _repository.UpdateAsync(todoItem);
        }
    }

    public async Task UpdateAsync(UpdateTodoItemDTO updatedTodoItemDTO)
    {
        TodoItem todoItemUpdated = _mapper.Map<TodoItem>(updatedTodoItemDTO);
        await _repository.UpdateAsync(todoItemUpdated);
    }

    public class TodoItemComparer : IComparer<TodoItem>
    {
        public int Compare(TodoItem? x, TodoItem? y)
        {
            int xValue =  x!.Completed ? 1 : 0;
            int yValue = y!.Completed ? 1 : 0;
            if (xValue > yValue) return 1;
            else if (xValue < yValue) return -1;
            else return x!.DueDate.CompareTo(y!.DueDate);
        }
    }

    
}