using TodoListAppMVC.DAL.Models;
using System.Xml.Linq;

namespace TodoListAppMVC.DAL.Repositories;

public class CategoryRepositoryXML : ICategoryRepository
{
    private readonly string _xmlCategoriesFileName;
    private readonly string _xmlTodoItemsFileName;
    public CategoryRepositoryXML(IConfiguration config)
    {
        _xmlCategoriesFileName = config["CategoriesXMLFilePath"];
        _xmlTodoItemsFileName = config["TodoItemsXMLFilePath"];
    }

    public void Add(Category category)
    {
        XDocument xdoc = XDocument.Load(_xmlCategoriesFileName);
        XElement? categories = xdoc.Element("categories");
        categories?.Add(new XElement("category", 
                            new XAttribute("id", GetNextId(xdoc)), 
                            new XAttribute("name", category.Name!)));
        xdoc.Save(_xmlCategoriesFileName);
    }

    public void DeleteById(int id)
    {
        // var todoItemsToDelete = _todoItemRepository.GetByCategoryId(id);
        // foreach (var todoItemToDelete in todoItemsToDelete)
        // {
        //     _todoItemRepository.DeleteById(todoItemToDelete.Id);
        //     Console.WriteLine(todoItemToDelete.Id);
        // }
        XDocument xdoc = XDocument.Load(_xmlTodoItemsFileName);
        var todoItemsToDelete = xdoc.Element("todoitems")?.Elements("todoitem")
            .Where(todoItemElem => todoItemElem.Attribute("categoryId")!.Value == id.ToString());
        if (todoItemsToDelete is not null)
        {
            foreach (var todoItem in todoItemsToDelete)
            {
                todoItemsToDelete.Remove();
            }
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
        }
    }

    public IEnumerable<Category> GetAll()
    {
        XDocument xdoc = XDocument.Load(_xmlCategoriesFileName);
        XElement? categories = xdoc.Element("categories");
        
        if (categories is null)
            throw new FormatException("Invalid xml format!");
        return categories.Elements("category")
            .Select(c => new Category 
                    { 
                        Id = Convert.ToInt32(c.Attribute("id")!.Value), 
                        Name = c.Attribute("name")!.Value 
                    });    
    }

    public Category? GetByName(string name)
    {
        XDocument xdoc = XDocument.Load(_xmlCategoriesFileName);
        XElement? categories = xdoc.Element("categories");
        
        if (categories is null)
            throw new FormatException("Invalid xml format!");
        return categories.Elements("category")
            .Where(c => c.Attribute("name")?.Value == name)
            .Select(c => new Category 
                    { 
                        Id = Convert.ToInt32(c.Attribute("id")?.Value), 
                        Name = c.Attribute("name")?.Value 
                    })
            .FirstOrDefault();    
    }

    public Category? GetById(int id)
    {
        XDocument xdoc = XDocument.Load(_xmlCategoriesFileName);
        XElement? categories = xdoc.Element("categories");
        
        if (categories is null)
            throw new FormatException("Invalid xml format!");
        return categories.Elements("category")
            .Where(c => c.Attribute("id")?.Value == id.ToString())
            .Select(c => new Category 
                    { 
                        Id = Convert.ToInt32(c.Attribute("id")!.Value), 
                        Name = c.Attribute("name")!.Value 
                    })
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