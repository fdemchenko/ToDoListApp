using GraphQL.Types;
using GraphQL;
using TodoListDAL.Models;
using TodoListDAL.Repositories;
using TodoListGraphQLAPI.Types;

namespace TodoListGraphQLAPI.GraphQL.Mutations
{
    public partial class TodoListMutation
    {
        public void AddTodoItemMutations()
        {
            Field<TodoItemType>("createTodoItem")
            .Argument<NonNullGraphType<TodoItemInputType>>("todoItem")
            .Resolve(context =>
            {
                var repository = context.RequestServices!.GetRequiredService<ITodoItemRepository>();
                TodoItem newTodoItem = context.GetArgument<TodoItem>("todoItem");
                if (string.IsNullOrWhiteSpace(newTodoItem.Name))
                    throw new ExecutionError("Todoitem name cannot be empty");
                return repository.Add(newTodoItem);
            });
            Field<TodoItemType>("deleteTodoItem")
                .Argument<NonNullGraphType<IdGraphType>>("id")
                .Resolve(context =>
                {
                    var repository = context.RequestServices!.GetRequiredService<ITodoItemRepository>();
                    return repository.DeleteById(context.GetArgument<int>("id"));
                });
            Field<TodoItemType>("updateTodoItem")
                .Argument<NonNullGraphType<TodoItemInputType>>("updatedTodoItem")
                .Argument<NonNullGraphType<IdGraphType>>("id")
                .Resolve(context =>
                {
                    var repository = context.RequestServices!.GetRequiredService<ITodoItemRepository>();
                    TodoItem updatedTodoItem = context.GetArgument<TodoItem>("updatedTodoItem");
                    if (string.IsNullOrWhiteSpace(updatedTodoItem.Name))
                        throw new ExecutionError("Todoitem name cannot be empty");
                    updatedTodoItem.Id = context.GetArgument<int>("id");
                    return repository.Update(updatedTodoItem);
                });
        }
    }
}
