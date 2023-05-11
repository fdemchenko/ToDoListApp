using GraphQL;
using TodoListGraphQLAPI;
using TodoListDAL.Repositories;
using TodoListDAL.AutoMapperConfig;
using TodoListGraphQLAPI.Types;
using TodoListGraphQLAPI.TodolistSchema;
using TodoListGraphQLAPI.GraphQL.Mutations;
using TodoListGraphQLAPI.GraphQL.Queries;



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

    httpContext.Request.Headers.TryGetValue(StorageTypes.HeaderName, out var storageType);
    if (storageType.FirstOrDefault() == StorageTypes.XmlType)
        return provider.GetRequiredService<CategoryRepositoryXML>();
    else if (storageType.FirstOrDefault() == StorageTypes.DatabaseType)
        return provider.GetRequiredService<CategoryRepositoryDapper>();
    else
        throw new ArgumentOutOfRangeException("Invalid storage type");
});
builder.Services.AddScoped<ITodoItemRepository>(provider => {
    HttpContext httpContext = provider.GetRequiredService<IHttpContextAccessor>().HttpContext!;

    httpContext.Request.Headers.TryGetValue(StorageTypes.HeaderName, out var storageType);
    if (storageType.FirstOrDefault() == StorageTypes.XmlType)
        return provider.GetRequiredService<TodoItemRepositoryXML>();
    else if (storageType.FirstOrDefault() == StorageTypes.DatabaseType)
        return provider.GetRequiredService<TodoItemRepositoryDapper>();
    else
        throw new ArgumentOutOfRangeException("Invalid storage type");
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

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseGraphQL();
app.UseGraphQLPlayground();

app.Run();
