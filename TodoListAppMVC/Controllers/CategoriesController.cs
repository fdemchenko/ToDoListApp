using Microsoft.AspNetCore.Mvc;
using TodoListAppMVC.Models;
using TodoListAppMVC.Services;
using TodoListAppMVC.DAL.Models;
using TodoListAppMVC.DTO;

namespace TodoListAppMVC.Controllers;
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        IEnumerable<Category> categories = _categoryService.GetAll();
        
        return View(new CategoryCreationViewModel { Categories = categories });
    }

    [HttpPost]
    public IActionResult CreateCategory(IncomingCategoryDTO NewCategory)
    {
        if (ModelState.IsValid)
            _categoryService.Add(NewCategory);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult DeleteCategory(int id)
    {
        _categoryService.DeleteById(id);
        return RedirectToAction(nameof(Index));
    }
}