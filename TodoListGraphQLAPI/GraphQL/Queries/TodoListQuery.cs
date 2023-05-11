using GraphQL.Types;

namespace TodoListGraphQLAPI.GraphQL.Queries;

public partial class TodoListQuery : ObjectGraphType
{
    public TodoListQuery()
    {
        AddCategoriesQueries();
        AddTodoItemQueries();
    }
}