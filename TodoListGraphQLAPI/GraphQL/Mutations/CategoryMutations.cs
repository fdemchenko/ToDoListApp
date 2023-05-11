using GraphQL.Types;
using GraphQL;
using TodoListDAL.Models;
using TodoListDAL.Repositories;
using TodoListGraphQLAPI.Types;

namespace TodoListGraphQLAPI.GraphQL.Mutations
{
    public partial class TodoListMutation
    {
        public void AddCategoryMutations()
        {
            Field<CategoryType>("createCategory")
            .Argument<NonNullGraphType<CategoryInputType>>("category")
            .Resolve(context =>
            {
                var repository = context.RequestServices!.GetRequiredService<ICategoryRepository>();
                Category newCategory = context.GetArgument<Category>("category");
                if (string.IsNullOrWhiteSpace(newCategory.Name))
                    throw new ExecutionError("Category name cannot be empty");
                return repository.Add(newCategory);
            });
            Field<CategoryType>("deleteCategory")
                .Argument<NonNullGraphType<IdGraphType>>("id")
                .Resolve(context =>
                {
                    var repository = context.RequestServices!.GetRequiredService<ICategoryRepository>();
                    return repository.DeleteById(context.GetArgument<int>("id"));
                });
        }
    }
}
