using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoListAppMVC.Models;
using TodoListAppMVC.Services;
// using TodoListAppMVC.DAL.Models;
using TodoListDAL.Models;
using TodoListAppMVC.DTO;
using AutoMapper;

namespace TodoListAppMVC.Controllers;
public class TodoItemsController : Controller
{
    private readonly ITodoItemService _todoItemService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public TodoItemsController(ITodoItemService todoItemService, ICategoryService categoryService, IMapper mapper)
    {
        _todoItemService = todoItemService;
        _categoryService = categoryService;
        _mapper = mapper;
    }
    [HttpGet]
    public IActionResult Index()
    {
        IEnumerable<Category> categories = _categoryService.GetAll();
        TodoItemCreationViewModel viewModel = new TodoItemCreationViewModel();
        viewModel.TodoItems = _todoItemService.GetAllSorted();
        viewModel.Categories = categories.Select(category => new SelectListItem { Value = category.Id.ToString(), Text = category.Name } );

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult CreateTodoItem(IncomingTodoItemDTO NewTodoItem)
    {
        if (ModelState.IsValid)
            _todoItemService.Add(NewTodoItem);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult DeleteTodoItem(int id)
    {
        _todoItemService.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult SetTodoItemCompleted(int id)
    {
        _todoItemService.SetCompleted(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult UpdateTodoItem(int id)
    {
        TodoItem? todoItem =  _todoItemService.GetById(id);
        if (todoItem is null)
            return RedirectToAction(nameof(Index));
        
        IEnumerable<Category> categories = _categoryService.GetAll();

        TodoItemUpdateViewModel viewModel = new TodoItemUpdateViewModel();
        viewModel.TodoItemToUpdate = _mapper.Map<UpdateTodoItemDTO>(todoItem);
        viewModel.Categories = categories.Select(category => new SelectListItem { Value = category.Id.ToString(), Text = category.Name } );

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult UpdateTodoItemPost(UpdateTodoItemDTO TodoItemToUpdate)
    {
        if (ModelState.IsValid)
             _todoItemService.Update(TodoItemToUpdate);

        return RedirectToAction(nameof(Index));
    }
}