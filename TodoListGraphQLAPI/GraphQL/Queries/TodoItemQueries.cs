using GraphQL.Types;
using GraphQL;
using TodoListGraphQLAPI.Types;
using TodoListDAL.Repositories;

namespace TodoListGraphQLAPI.GraphQL.Queries;

public partial class TodoListQuery : ObjectGraphType
{
    public void AddTodoItemQueries()
    {
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