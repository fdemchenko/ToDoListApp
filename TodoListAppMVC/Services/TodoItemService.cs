using TodoListDAL.Models;
using TodoListAppMVC.DTO;
using TodoListDAL.Repositories;
using AutoMapper;

namespace TodoListAppMVC.Services;

public class TodoItemService : ITodoItemService
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    public TodoItemService(ITodoItemRepository todoItemRepository, 
        IMapper mapper, ICategoryRepository categoryRepository)
    {
        _todoItemRepository = todoItemRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    public void Add(IncomingTodoItemDTO newTodoItemDTO)
    {
        TodoItem newItem = _mapper.Map<TodoItem>(newTodoItemDTO);
        if (_categoryRepository.GetById(newTodoItemDTO.CategoryId) is not null)
            _todoItemRepository.Add(newItem);
    }

    public void Delete(int id)
    {
        _todoItemRepository.DeleteById(id);
    }

    public IEnumerable<TodoItem> GetAllSorted()
    {
        IEnumerable<TodoItem> items = _todoItemRepository.GetAll();
        return items.OrderBy(item => item, new TodoItemComparer());
    }

    public TodoItem? GetById(int id)
    {
        return _todoItemRepository.GetById(id);
    }

    public void SetCompleted(int id)
    {
        TodoItem? todoItem = _todoItemRepository.GetById(id);
        if (todoItem is not null) 
        {
            todoItem.Completed = true;
            _todoItemRepository.Update(todoItem);
        }
    }

    public void Update(UpdateTodoItemDTO updatedTodoItemDTO)
    {
        TodoItem todoItemUpdated = _mapper.Map<TodoItem>(updatedTodoItemDTO);
        _todoItemRepository.Update(todoItemUpdated);
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