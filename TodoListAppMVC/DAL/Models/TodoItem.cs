namespace TodoListAppMVC.DAL.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime DueDate { get; set; }
    public bool Completed { get; set; }
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
}