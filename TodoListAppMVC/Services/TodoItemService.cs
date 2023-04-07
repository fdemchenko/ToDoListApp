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
    public async Task Add(IncomingTodoItemDTO newTodoItemDTO)
    {
        TodoItem newItem = _mapper.Map<TodoItem>(newTodoItemDTO);
        await _repository.Add(newItem);
    }

    public async Task Delete(int id)
    {
        await _repository.DeleteById(id);
    }

    public async Task<IEnumerable<TodoItem>> GetAllSorted()
    {
        IEnumerable<TodoItem> items = await _repository.GetAll();
        return items.OrderBy(item => item, new TodoItemComparer());
    }

    public Task<TodoItem?> GetById(int id)
    {
        return _repository.GetById(id);
    }

    public async Task SetCompleted(int id)
    {
        TodoItem? todoItem = await _repository.GetById(id);
        if (todoItem is not null) 
        {
            todoItem.Completed = true;
            await _repository.Update(todoItem);
        }
    }

    public async Task Update(UpdateTodoItemDTO updatedTodoItemDTO)
    {
        TodoItem todoItemUpdated = _mapper.Map<TodoItem>(updatedTodoItemDTO);
        await _repository.Update(todoItemUpdated);
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