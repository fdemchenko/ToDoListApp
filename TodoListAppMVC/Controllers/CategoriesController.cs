using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public async Task<IActionResult> Index()
    {
        IEnumerable<Category> categories = await _categoryService.GetAll();
        
        return View(new CategoryCreationViewModel { Categories = categories });
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(IncomingCategoryDTO NewCategory)
    {
        if (ModelState.IsValid)
            await _categoryService.Add(NewCategory);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteById(id);
        return RedirectToAction(nameof(Index));
    }
}