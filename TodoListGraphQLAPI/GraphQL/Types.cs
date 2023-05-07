using GraphQL.Types;
using TodoListDAL.Models;

namespace TodoListGraphQLAPI.Types
{
    public class CategoryType : ObjectGraphType<Category>
    {
        public CategoryType()
        {
            Field(x => x.Id).Description("Category id");
            Field(x => x.Name).Description("Category name");
        }
    }

    public class CategoryInputType : InputObjectGraphType<Category>
    {
        public CategoryInputType()
        {
            Field(x => x.Name).Description("Category name");
        }   
    }

    public class TodoItemInputType : InputObjectGraphType<TodoItem>
    {
        public TodoItemInputType()
        {
            Field(x => x.Name).Description("Todo item name");
            Field(x => x.DueDate).Description("Todo item due date");
            Field(x => x.Completed).Description("Is todo item completed");
            Field(x => x.CategoryId).Description("Todo item category id");
        }
    }

    public class TodoItemType : ObjectGraphType<TodoItem>
    {
        public TodoItemType()
        {
            Field(x => x.Id).Description("Todo item ID");
            Field(x => x.Name).Description("Todo item name");
            Field(x => x.DueDate).Description("Todo item due date");
            Field(x => x.Completed).Description("Is todoitem completed");
            Field(x => x.CategoryId).Description("Todo item category id");
            Field<CategoryType>("category").Description("Todoietm category");
        }
    }
}

