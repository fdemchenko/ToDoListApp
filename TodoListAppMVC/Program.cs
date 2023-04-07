using TodoListAppMVC.DAL.Repositories;
using TodoListAppMVC.Services;
using TodoListAppMVC.DAL.Models;
using TodoListAppMVC.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAutoMapper(config => {
    config.CreateMap<IncomingTodoItemDTO, TodoItem>();
    config.CreateMap<IncomingCategoryDTO, Category>();
    config.CreateMap<UpdateTodoItemDTO, TodoItem>().ReverseMap();
});
builder.Services.AddSingleton<ITodoItemRepository, TodoItemRepositoryDapper>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepositoryDapper>();
builder.Services.AddSingleton<ITodoItemService, TodoItemService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TodoItems}/{action=Index}/{id?}");

app.Run();
