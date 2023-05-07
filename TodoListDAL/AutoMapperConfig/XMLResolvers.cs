using AutoMapper;
using System.Xml.Linq;
using TodoListDAL.Models;

namespace TodoListDAL.AutoMapperConfig
{
    public class XAttributeCategoryResolver<T> : IValueResolver<XElement, Category, T?>
    {
        public string AttributeName { get; set; }
        public XAttributeCategoryResolver(string attributeName)
        {
            AttributeName = attributeName;
        }

        public T? Resolve(XElement source, Category destination, T? destMember, ResolutionContext context)
        {
            if (source == null)
                return default(T);
            XAttribute? attribute = source.Attribute(AttributeName);
            if (attribute == null || String.IsNullOrEmpty(attribute.Value))
                return default(T);
            return (T) Convert.ChangeType(attribute.Value, typeof(T));
        }
    }

    public class XAttributeTodoItemResolver<T> : IValueResolver<XElement, TodoItem, T?>
    {
        public string AttributeName { get; set; }
        public XAttributeTodoItemResolver(string attributeName)
        {
            AttributeName = attributeName;
        }

        public T? Resolve(XElement source, TodoItem destination, T? destMember, ResolutionContext context)
        {
            if (source == null)
                return default(T);
            XAttribute? attribute = source.Attribute(AttributeName);
            if (attribute == null || String.IsNullOrEmpty(attribute.Value))
                return default(T);
            return (T) Convert.ChangeType(attribute.Value, typeof(T));
        }
    }

    public class XElementTodoItemResolver<T> : IValueResolver<XElement, TodoItem, T?>
    {
        public string ElementName { get; set; }
        public XElementTodoItemResolver(string elementName)
        {
            ElementName = elementName;
        }

        public T? Resolve(XElement source, TodoItem destination, T? destMember, ResolutionContext context)
        {
            if (source == null)
                return default(T);
            XElement? element = source.Element(ElementName);
            if (element == null || String.IsNullOrEmpty(element.Value))
                return default(T);
            return (T) Convert.ChangeType(element.Value, typeof(T));
        }
    }
}


