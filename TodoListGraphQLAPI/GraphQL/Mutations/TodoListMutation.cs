using GraphQL.Types;

namespace TodoListGraphQLAPI.GraphQL.Mutations;

public partial class TodoListMutation : ObjectGraphType
{
    public TodoListMutation()
    {
        AddCategoryMutations();
        AddTodoItemMutations();
    }
}