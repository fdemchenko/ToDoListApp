using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoListAppMVC.Models;
using TodoListAppMVC.Services;
using TodoListAppMVC.DAL.Models;
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
    public async Task<IActionResult> Index()
    {
        IEnumerable<Category> categories = await _categoryService.GetAll();
        TodoItemCreationViewModel viewModel = new TodoItemCreationViewModel();
        viewModel.TodoItems = await _todoItemService.GetAllSorted();
        viewModel.Categories = categories.Select(category => new SelectListItem { Value = category.Id.ToString(), Text = category.Name } );

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoItem(IncomingTodoItemDTO NewTodoItem)
    {
        if (ModelState.IsValid)
            await _todoItemService.Add(NewTodoItem);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        await _todoItemService.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> SetTodoItemCompleted(int id)
    {
        await _todoItemService.SetCompleted(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> UpdateTodoItem(int id)
    {
        TodoItem? todoItem = await _todoItemService.GetById(id);
        if (todoItem is null)
            return RedirectToAction(nameof(Index));
        
        IEnumerable<Category> categories = await _categoryService.GetAll();

        TodoItemUpdateViewModel viewModel = new TodoItemUpdateViewModel();
        viewModel.TodoItemToUpdate = _mapper.Map<UpdateTodoItemDTO>(todoItem);
        viewModel.Categories = categories.Select(category => new SelectListItem { Value = category.Id.ToString(), Text = category.Name } );

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTodoItemPost(UpdateTodoItemDTO TodoItemToUpdate)
    {
        if (ModelState.IsValid)
            await _todoItemService.Update(TodoItemToUpdate);

        return RedirectToAction(nameof(Index));
    }
}