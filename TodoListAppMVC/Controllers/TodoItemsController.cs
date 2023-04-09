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
    public async Task<IActionResult> IndexAsync()
    {
        IEnumerable<Category> categories = await _categoryService.GetAllAsync();
        TodoItemCreationViewModel viewModel = new TodoItemCreationViewModel();
        viewModel.TodoItems = await _todoItemService.GetAllSortedAsync();
        viewModel.Categories = categories.Select(category => new SelectListItem { Value = category.Id.ToString(), Text = category.Name } );

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoItemAsync(IncomingTodoItemDTO NewTodoItem)
    {
        if (ModelState.IsValid)
            await _todoItemService.AddAsync(NewTodoItem);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteTodoItemAsync(int id)
    {
        await _todoItemService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> SetTodoItemCompletedAsync(int id)
    {
        await _todoItemService.SetCompletedAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> UpdateTodoItemAsync(int id)
    {
        TodoItem? todoItem = await _todoItemService.GetByIdAsync(id);
        if (todoItem is null)
            return RedirectToAction(nameof(Index));
        
        IEnumerable<Category> categories = await _categoryService.GetAllAsync();

        TodoItemUpdateViewModel viewModel = new TodoItemUpdateViewModel();
        viewModel.TodoItemToUpdate = _mapper.Map<UpdateTodoItemDTO>(todoItem);
        viewModel.Categories = categories.Select(category => new SelectListItem { Value = category.Id.ToString(), Text = category.Name } );

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTodoItemPostAsync(UpdateTodoItemDTO TodoItemToUpdate)
    {
        if (ModelState.IsValid)
            await _todoItemService.UpdateAsync(TodoItemToUpdate);

        return RedirectToAction(nameof(Index));
    }
}