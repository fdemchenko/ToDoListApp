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
    public void Add(IncomingTodoItemDTO newTodoItemDTO)
    {
        TodoItem newItem = _mapper.Map<TodoItem>(newTodoItemDTO);
        _repository.Add(newItem);
    }

    public void Delete(int id)
    {
        _repository.DeleteById(id);
    }

    public IEnumerable<TodoItem> GetAllSorted()
    {
        IEnumerable<TodoItem> items = _repository.GetAll();
        return items.OrderBy(item => item, new TodoItemComparer());
    }

    public TodoItem? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public void SetCompleted(int id)
    {
        TodoItem? todoItem = _repository.GetById(id);
        if (todoItem is not null) 
        {
            todoItem.Completed = true;
            _repository.Update(todoItem);
        }
    }

    public void Update(UpdateTodoItemDTO updatedTodoItemDTO)
    {
        TodoItem todoItemUpdated = _mapper.Map<TodoItem>(updatedTodoItemDTO);
        _repository.Update(todoItemUpdated);
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