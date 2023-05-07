
// using TodoListAppMVC.DAL.Repositories;
using TodoListAppMVC.Services;
// using TodoListAppMVC.DAL.Models;
using TodoListAppMVC.DTO;
using TodoListDAL.AutoMapperConfig;
using TodoListDAL.Models;
using TodoListDAL.Repositories;
// using TodoListAppMVC.AutoMapperConfig;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAutoMapper(config => {
    config.CreateMap<IncomingTodoItemDTO, TodoItem>();
    config.CreateMap<IncomingCategoryDTO, Category>();
    config.CreateMap<UpdateTodoItemDTO, TodoItem>().ReverseMap();
    config.AddProfile<XMLMappingProfile>();
});

builder.Services.AddTransient<CategoryRepositoryDapper>();
builder.Services.AddTransient<CategoryRepositoryXML>();
builder.Services.AddTransient<TodoItemRepositoryDapper>();
builder.Services.AddTransient<TodoItemRepositoryXML>();

builder.Services.AddScoped<ICategoryRepository>(provider => {
    HttpContext httpContext = provider.GetRequiredService<IHttpContextAccessor>().HttpContext!;

    string storageType = string.Empty;
    httpContext.Request.Cookies.TryGetValue("storage", out storageType!);
    storageType = storageType ?? "postgresql";

    if (storageType.Equals("xml", StringComparison.OrdinalIgnoreCase))
        return provider.GetRequiredService<CategoryRepositoryXML>();
    return provider.GetRequiredService<CategoryRepositoryDapper>();
});
builder.Services.AddScoped<ITodoItemRepository>(provider => {
    HttpContext httpContext = provider.GetRequiredService<IHttpContextAccessor>().HttpContext!;

    string storageType = string.Empty;
    httpContext.Request.Cookies.TryGetValue("storage", out storageType!);
    storageType = storageType ?? "postgresql";

    if (storageType.Equals("xml", StringComparison.OrdinalIgnoreCase))
        return provider.GetRequiredService<TodoItemRepositoryXML>();
    return provider.GetRequiredService<TodoItemRepositoryDapper>();
});

builder.Services.AddScoped<ITodoItemService, TodoItemService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
