using TodoListGraphQLAPI.GraphQL.Queries;
using GraphQL.Types;
using TodoListGraphQLAPI.GraphQL.Mutations;

namespace TodoListGraphQLAPI.TodolistSchema;

public class TodoListSchema : Schema
{
    public TodoListSchema(TodoListQuery query, TodoListMutation mutation)
    {
        Query = query;
        Mutation = mutation;  
    }
}