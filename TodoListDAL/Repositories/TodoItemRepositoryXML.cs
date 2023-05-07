using TodoListDAL.Models;
using TodoListDAL.Exceptions;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace TodoListDAL.Repositories;

public class TodoItemRepositoryXML : ITodoItemRepository
{
    private readonly string _xmlTodoItemsFileName;
    private readonly CategoryRepositoryXML _categoryRepository;
    private readonly IMapper _mapper;

    public TodoItemRepositoryXML(IConfiguration config, CategoryRepositoryXML categoryRepository, IMapper mapper)
    {
        _xmlTodoItemsFileName = config["TodoItemsXMLFilePath"] ??
            throw new InvalidConfigurationException("Cannot get todoitems xml file path");
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public TodoItem Add(TodoItem todoItem)
    {
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        XElement? todoitems = xdoc.Element("todoitems");

        int nextId = GetNextId(xdoc); 
        todoitems?.Add(new XElement("todoitem", 
                        new XAttribute("id", nextId), 
                        new XAttribute("categoryId", todoItem.CategoryId),
                        new XElement("name", todoItem.Name),
                        new XElement("dueDate", todoItem.DueDate),
                        new XElement("completed", todoItem.Completed.ToString())));
        xdoc.Save(_xmlTodoItemsFileName);
        todoItem.Id = nextId;
        todoItem.Category = _categoryRepository.GetById(todoItem.CategoryId);
        return todoItem;
    }

    public TodoItem? DeleteById(int id)
    { 
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        XElement? todoitems = xdoc.Element("todoitems");

        var todoitemToDelete = todoitems?.Elements("todoitem")
            ?.FirstOrDefault(c => c.Attribute("id")?.Value == id.ToString());
        if (todoitemToDelete is not null)
        {
            todoitemToDelete.Remove();
            xdoc.Save(_xmlTodoItemsFileName);
            TodoItem todoItem = _mapper.Map<TodoItem>(todoitemToDelete);
            todoItem.Category = _categoryRepository.GetById(todoItem.CategoryId);
            return todoItem;
        }
        return null;
    }

    public IEnumerable<TodoItem> GetAll()
    {
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        XElement? todoitems = xdoc.Element("todoitems");
        
        if (todoitems is null)
            throw new FormatException("Invalid xml format!");
        return todoitems.Elements("todoitem")
            .Select(todoItemElement => {
                TodoItem todoItem = _mapper.Map<TodoItem>(todoItemElement);
                todoItem.Category = _categoryRepository.GetById(todoItem.CategoryId);
                return todoItem;
            }).OrderBy(todoItem => todoItem.Completed).ThenBy(todoItem => todoItem.DueDate);
    }

    public TodoItem? GetById(int id)
    {
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        XElement? todoitems = xdoc.Element("todoitems");
        
        if (todoitems is null)
            throw new FormatException("Invalid xml format!");   
        return todoitems.Elements("todoitem")
            .Where(todoItemElement => todoItemElement.Attribute("id")!.Value == id.ToString())
            .Select(todoItemElement => {
                TodoItem todoItem = _mapper.Map<TodoItem>(todoItemElement);
                todoItem.Category = _categoryRepository.GetById(todoItem.CategoryId);
                return todoItem;
            })
            .FirstOrDefault();
    }

    public TodoItem? Update(TodoItem todoItem)
    {
        int todoItemId = todoItem.Id;
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        XElement? todoItemToUpdate = xdoc.Element("todoitems")?.Elements("todoitem")
            ?.Where(todoItem => todoItem.Attribute("id")?.Value == todoItemId.ToString())
            ?.FirstOrDefault();
        if (todoItemToUpdate is null)
            return null;

        todoItemToUpdate.Attribute("categoryId")!.Value = todoItem.CategoryId.ToString();
        todoItemToUpdate.Element("completed")!.Value = todoItem.Completed.ToString();
        todoItemToUpdate.Element("dueDate")!.Value = todoItem.DueDate.ToString("yyyy-MM-ddTHH:mm:ss");
        todoItemToUpdate.Element("name")!.Value = todoItem.Name!;

        xdoc.Save(_xmlTodoItemsFileName);
        todoItem.Category = _categoryRepository.GetById(todoItem.CategoryId);
        return todoItem;
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
            .Where(todoItemElement => todoItemElement.Attribute("categoryId")!.Value == categoryId.ToString())
            .Select(todoItemElement => {
                TodoItem todoItem = _mapper.Map<TodoItem>(todoItemElement);
                todoItem.Category = _categoryRepository.GetById(todoItem.CategoryId);
                return todoItem;
            });
    }
}