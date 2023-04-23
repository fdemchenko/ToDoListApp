using TodoListAppMVC.DAL.Models;
using System.Xml.Linq;

namespace TodoListAppMVC.DAL.Repositories;

public class TodoItemRepositoryXML : ITodoItemRepository
{
    private readonly string _xmlTodoItemsFileName;
    private readonly ICategoryRepository _categoryRepository;

    public TodoItemRepositoryXML(IConfiguration config, ICategoryRepository categoryRepository)
    {
        _xmlTodoItemsFileName = config["TodoItemsXMLFilePath"];
        _categoryRepository = categoryRepository;
    }

    public void Add(TodoItem todoItem)
    {
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        XElement? todoitems = xdoc.Element("todoitems");
        todoitems?.Add(new XElement("todoitem", 
                            new XAttribute("id", GetNextId(xdoc)), 
                            new XAttribute("categoryId", todoItem.CategoryId),
                            new XElement("name", todoItem.Name),
                            new XElement("dueDate", todoItem.DueDate),
                            new XElement("completed", todoItem.Completed.ToString())));
        xdoc.Save(_xmlTodoItemsFileName);
    }

    public void DeleteById(int id)
    { 
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        XElement? todoitems = xdoc.Element("todoitems");

        var todoitemToDelete = todoitems?.Elements("todoitem")
            ?.FirstOrDefault(c => c.Attribute("id")?.Value == id.ToString());
        if (todoitemToDelete is not null)
        {
            todoitemToDelete.Remove();
            xdoc.Save(_xmlTodoItemsFileName);
        }
    }

    public IEnumerable<TodoItem> GetAll()
    {
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        XElement? todoitems = xdoc.Element("todoitems");
        
        if (todoitems is null)
            throw new FormatException("Invalid xml format!");
        return todoitems.Elements("todoitem")
                .Select(t => new TodoItem 
                    { 
                        Id = Convert.ToInt32(t.Attribute("id")?.Value), 
                        Name = t.Element("name")?.Value,
                        DueDate = DateTime.Parse(t.Element("dueDate")!.Value),
                        Category = _categoryRepository.GetById(Convert.ToInt32(t.Attribute("categoryId")!.Value)),
                        Completed = Convert.ToBoolean(t.Element("completed")!.Value),
                        CategoryId = Convert.ToInt32(t.Attribute("categotyId")?.Value)
                    });
    }

    public TodoItem? GetById(int id)
    {
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        XElement? todoitems = xdoc.Element("todoitems");
        
        if (todoitems is null)
            throw new FormatException("Invalid xml format!");
        
        return todoitems.Elements("todoitem")
                .Where(t => t.Attribute("id")!.Value == id.ToString())
                .Select(t => { Console.WriteLine(t); return new TodoItem 
                    { 
                        Id = Convert.ToInt32(t.Attribute("id")?.Value), 
                        Name = t.Element("name")?.Value,
                        DueDate = DateTime.Parse(t.Element("dueDate")!.Value),
                        Category = _categoryRepository.GetById(Convert.ToInt32(t.Attribute("categoryId")!.Value)),
                        Completed = Convert.ToBoolean(t.Element("completed")!.Value),
                        CategoryId = Convert.ToInt32(t.Attribute("categoryId")!.Value)
                    };})
                .FirstOrDefault();    
    }

    public void Update(TodoItem todoItem)
    {
        int todoItemId = todoItem.Id;
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        XElement? todoItemToUpdate = xdoc.Element("todoitems")?.Elements("todoitem")
            ?.Where(todoItem => todoItem.Attribute("id")?.Value == todoItemId.ToString())
            ?.FirstOrDefault();
        if (todoItemToUpdate is null)
            return;

        todoItemToUpdate.Attribute("categoryId")!.Value = todoItem.CategoryId.ToString();
        todoItemToUpdate.Element("completed")!.Value = todoItem.Completed.ToString();
        todoItemToUpdate.Element("dueDate")!.Value = todoItem.DueDate.ToString("yyyy-MM-ddTHH:mm:ss");
        todoItemToUpdate.Element("name")!.Value = todoItem.Name!;

        xdoc.Save(_xmlTodoItemsFileName);
    }

    private int GetNextId(XDocument xdoc)
    {
        XElement? idElement = xdoc.Element("todoitems")?.Element("nextId");

        if (idElement is null)
            throw new FormatException("Invalid xml format!");
        int nextId = Convert.ToInt32(idElement.Value);

        idElement.Value = (nextId + 1).ToString();
        return nextId;
    }

    public IEnumerable<TodoItem> GetByCategoryId(int categoryId)
    {
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        XElement? todoitems = xdoc.Element("todoitems");
        
        if (todoitems is null)
            throw new FormatException("Invalid xml format!");
        return todoitems.Elements("todoitem")
            .Where(t => t.Attribute("categoryId")!.Value == categoryId.ToString())
            .Select(t => new TodoItem 
                { 
                    Id = Convert.ToInt32(t.Attribute("id")!.Value), 
                    Name = t.Element("name")!.Value,
                    DueDate = DateTime.Parse(t.Element("dueDate")!.Value),
                    Category = _categoryRepository.GetById(Convert.ToInt32(t.Attribute("categoryId")!.Value)),
                    Completed = Convert.ToBoolean(t.Element("completed")!.Value),
                    CategoryId = Convert.ToInt32(t.Attribute("categotyId")!.Value)
                });
    }
}