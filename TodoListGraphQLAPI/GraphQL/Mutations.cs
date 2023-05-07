using GraphQL.Types;
using GraphQL;
using TodoListDAL.Models;
using TodoListDAL.Repositories;
using TodoListGraphQLAPI.Types;

namespace TodoListGraphQLAPI.Mutations;

public class TodoListMutation : ObjectGraphType
{
    public TodoListMutation()
    {
        Field<CategoryType>("createCategory")
            .Argument<NonNullGraphType<CategoryInputType>>("category")
            .Resolve(context => {
                var repository = context.RequestServices!.GetRequiredService<ICategoryRepository>();    
                return repository.Add(context.GetArgument<Category>("category"));
            });
        Field<CategoryType>("deleteCategory")
            .Argument<NonNullGraphType<IdGraphType>>("id")
            .Resolve(context => {
                var repository = context.RequestServices!.GetRequiredService<ICategoryRepository>();
                return repository.DeleteById(context.GetArgument<int>("id"));
            });
        Field<TodoItemType>("createTodoItem")
            .Argument<NonNullGraphType<TodoItemInputType>>("todoItem")
            .Resolve(context => {
                var repository = context.RequestServices!.GetRequiredService<ITodoItemRepository>();
                return repository.Add(context.GetArgument<TodoItem>("todoItem"));
            });
        Field<TodoItemType>("deleteTodoItem")
            .Argument<NonNullGraphType<IdGraphType>>("id")
            .Resolve(context => {
                var repository = context.RequestServices!.GetRequiredService<ITodoItemRepository>();
                return repository.DeleteById(context.GetArgument<int>("id"));
            });
        Field<TodoItemType>("updateTodoItem")
            .Argument<NonNullGraphType<TodoItemInputType>>("updatedTodoItem")
            .Argument<NonNullGraphType<IdGraphType>>("id")
            .Resolve(context => {
                var repository = context.RequestServices!.GetRequiredService<ITodoItemRepository>();
                TodoItem updatedTodoItem = context.GetArgument<TodoItem>("updatedTodoItem");
                updatedTodoItem.Id = context.GetArgument<int>("id");
                return repository.Update(updatedTodoItem);
            });
    }
}