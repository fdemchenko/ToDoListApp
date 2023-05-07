using GraphQL.Types;
using GraphQL;
using TodoListGraphQLAPI.Types;
using TodoListDAL.Repositories;

namespace TodoListGraphQLAPI.Queries;

public class TodoListQuery : ObjectGraphType
{
    public TodoListQuery()
    {
        Field<ListGraphType<CategoryType>>("categories")
            .Resolve(context => {
                ICategoryRepository repository = context.RequestServices!.GetRequiredService<ICategoryRepository>();
                return repository.GetAll();
            });
        Field<CategoryType>("category")
            .Argument<IdGraphType>("id")
            .Resolve(context => {
                var id = context.GetArgument<int>("id");
                ICategoryRepository repository = context.RequestServices!.GetRequiredService<ICategoryRepository>();
                return repository.GetById(id);
            });
        Field<ListGraphType<TodoItemType>>("todoitems")
            .Resolve(context => {
                var repository = context.RequestServices!.GetRequiredService<ITodoItemRepository>();
                return repository.GetAll();
            });
        Field<TodoItemType>("todoitem")
            .Argument<IdGraphType>("id")
            .Resolve(context => {
                var id = context.GetArgument<int>("id");
                var repository = context.RequestServices!.GetRequiredService<ITodoItemRepository>();
                return repository.GetById(id);
            });
    }
}