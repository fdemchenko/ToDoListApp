using TodoListGraphQLAPI.Queries;
using TodoListGraphQLAPI.Mutations;
using GraphQL.Types;

namespace TodoListGraphQLAPI.TodolistSchema;

public class TodoListSchema : Schema
{
    public TodoListSchema(TodoListQuery query, TodoListMutation mutation)
    {
        Query = query;
        Mutation = mutation;
    }
}