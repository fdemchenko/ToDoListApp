using GraphQL.Types;
using GraphQL;
using TodoListGraphQLAPI.Types;
using TodoListDAL.Repositories;

namespace TodoListGraphQLAPI.GraphQL.Queries;

public partial class TodoListQuery : ObjectGraphType
{
    public void AddCategoriesQueries()
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
    }
}