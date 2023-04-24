using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TodoListAppMVC.Controllers;
public class StorageTypeController : Controller
{
    [HttpPost]
    public IActionResult ChangeStorageType(string storage)
    {
        HttpContext.Response.Cookies.Append("storage", storage);
        return RedirectToAction("Index", "TodoItems");
    }
}