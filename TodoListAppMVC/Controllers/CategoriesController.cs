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
    public async Task<IActionResult> IndexAsync()
    {
        IEnumerable<Category> categories = await _categoryService.GetAllAsync();
        
        return View(new CategoryCreationViewModel { Categories = categories });
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategoryAsync(IncomingCategoryDTO NewCategory)
    {
        if (ModelState.IsValid)
            await _categoryService.AddAsync(NewCategory);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteCategoryAsync(int id)
    {
        await _categoryService.DeleteByIdAsync(id);
        return RedirectToAction(nameof(Index));
    }
}