using GraphQL;
using TodoListDAL.Repositories;
using TodoListDAL.AutoMapperConfig;
using TodoListGraphQLAPI.Types;
using TodoListGraphQLAPI.Queries;
using TodoListGraphQLAPI.Mutations;
using TodoListGraphQLAPI.TodolistSchema;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(config => {
    config.AddProfile<XMLMappingProfile>();
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<CategoryRepositoryDapper>();
builder.Services.AddSingleton<CategoryRepositoryXML>();
builder.Services.AddSingleton<TodoItemRepositoryDapper>();
builder.Services.AddSingleton<TodoItemRepositoryXML>();

builder.Services.AddScoped<ICategoryRepository>(provider => {
    HttpContext httpContext = provider.GetRequiredService<IHttpContextAccessor>().HttpContext!;

    httpContext.Request.Headers.TryGetValue("Storage-Type", out var storageType);
    if (storageType.FirstOrDefault() == "XML")
        return provider.GetRequiredService<CategoryRepositoryXML>();
    return provider.GetRequiredService<CategoryRepositoryDapper>();
});
builder.Services.AddScoped<ITodoItemRepository>(provider => {
    HttpContext httpContext = provider.GetRequiredService<IHttpContextAccessor>().HttpContext!;

    httpContext.Request.Headers.TryGetValue("Storage-Type", out var storageType);
    if (storageType.FirstOrDefault() == "XML")
        return provider.GetRequiredService<TodoItemRepositoryXML>();
    return provider.GetRequiredService<TodoItemRepositoryDapper>();
});

builder.Services.AddSingleton<CategoryType>();
builder.Services.AddSingleton<TodoItemType>();
builder.Services.AddSingleton<TodoListQuery>();
builder.Services.AddSingleton<TodoListMutation>();
builder.Services.AddGraphQL(builder => builder
    .AddSystemTextJson()
    .AddSchema<TodoListSchema>()
);


var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseGraphQL();
app.UseGraphQLPlayground();

app.Run();
