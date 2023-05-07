using TodoListDAL.Models;
using TodoListDAL.Exceptions;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace TodoListDAL.Repositories;

public class CategoryRepositoryXML : ICategoryRepository
{
    private readonly string _xmlCategoriesFileName;
    private readonly string _xmlTodoItemsFileName;
    private readonly IMapper _mapper;
    public CategoryRepositoryXML(IConfiguration config, IMapper mapper)
    {
        _xmlCategoriesFileName = config["CategoriesXMLFilePath"] ?? 
            throw new InvalidConfigurationException("Cannot get categories xml file path");
        _xmlTodoItemsFileName = config["TodoItemsXMLFilePath"] ??
            throw new InvalidConfigurationException("Cannot get todoitems xml file path");
        _mapper = mapper;
    }

    public Category Add(Category category)
    {
        XDocument xdoc = XDocument.Load(_xmlCategoriesFileName);
        XElement? categories = xdoc.Element("categories");
        int nextId = GetNextId(xdoc);
        xdoc.Element("categories")?.Add(new XElement("category", 
                            new XAttribute("id", nextId),  
                            new XAttribute("name", category.Name!)));
        xdoc.Save(_xmlCategoriesFileName);
        category.Id = nextId;
        return category;
    } 

    public Category? DeleteById(int id)
    {
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        var todoItemsToDelete = xdoc.Element("todoitems")?.Elements("todoitem")
            .Where(todoItemElem => todoItemElem.Attribute("categoryId")!.Value == id.ToString());
        if (todoItemsToDelete is not null)
        {
            foreach (var todoItem in todoItemsToDelete)
                todoItemsToDelete.Remove();
        }
        xdoc.Save(_xmlTodoItemsFileName);


        xdoc = XDocument.Load(_xmlCategoriesFileName);
        XElement? categories = xdoc.Element("categories");
 
        var categoryToDelete = categories?.Elements("category")
            ?.FirstOrDefault(c => c.Attribute("id")?.Value == id.ToString());

        if (categoryToDelete is not null) 
        {
            categoryToDelete.Remove();
            xdoc.Save(_xmlCategoriesFileName);
            return _mapper.Map<Category>(categoryToDelete);
        }
        return null;
    }

    public IEnumerable<Category> GetAll()
    {
        XDocument xdoc = XDocument.Load(_xmlCategoriesFileName);
        XElement? categories = xdoc.Element("categories");
        
        if (categories is null)
            throw new FormatException("Invalid xml format!");
        return categories.Elements("category")
               .Select(categoryElement => _mapper.Map<Category>(categoryElement));  
    }

    public Category? GetByName(string name)
    {
        XDocument xdoc = XDocument.Load(_xmlCategoriesFileName);
        XElement? categories = xdoc.Element("categories");

        if (categories is null)
            throw new FormatException("Invalid xml format!");
        return categories.Elements("category")
            .Where(categoryElement => categoryElement.Attribute("name")?.Value == name)
            .Select(categoryElement => _mapper.Map<Category>(categoryElement))
            .FirstOrDefault();
    }

    public Category? GetById(int id)
    {
        XDocument xdoc = XDocument.Load(_xmlCategoriesFileName);
        XElement? categories = xdoc.Element("categories");
        
        if (categories is null)
            throw new FormatException("Invalid xml format!");
        return categories.Elements("category")
            .Where(categoryElement => categoryElement.Attribute("id")?.Value == id.ToString())
            .Select(categoryElement => _mapper.Map<Category>(categoryElement))
            .FirstOrDefault();    
    }

    private int GetNextId(XDocument xdoc)
    {
        XElement? idElement = xdoc.Element("categories")?.Element("nextId");

        if (idElement is null)
            throw new FormatException("Invalid xml format!");
        int nextId = Convert.ToInt32(idElement.Value);

        idElement.Value = (nextId + 1).ToString();
        return nextId;
    }
}