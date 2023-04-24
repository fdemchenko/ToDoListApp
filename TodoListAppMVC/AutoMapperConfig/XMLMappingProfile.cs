using AutoMapper;
using System.Xml.Linq;
using TodoListAppMVC.DAL.Models;

namespace TodoListAppMVC.AutoMapperConfig;

public class XMLMappingProfile : Profile
{
    public XMLMappingProfile()
    {
        CreateMap<XElement, Category>()
            .ForMember(dest => dest.Id,
                       opt => opt.MapFrom(new XAttributeCategoryResolver<int>("id")))
            .ForMember(dest => dest.Name,
                       opt =>  opt.MapFrom(new XAttributeCategoryResolver<string>("name")));

        CreateMap<XElement, TodoItem>()
            .ForMember(dest => dest.Id,
                       opt => opt.MapFrom(new XAttributeTodoItemResolver<int>("id")))
            .ForMember(dest => dest.CategoryId,
                       opt =>  opt.MapFrom(new XAttributeTodoItemResolver<int>("categoryId")))
            .ForMember(dest => dest.DueDate,
                       opt =>  opt.MapFrom(new XElementTodoItemResolver<DateTime>("dueDate")))
            .ForMember(dest => dest.Name,
                       opt =>  opt.MapFrom(new XElementTodoItemResolver<string>("name")))
            .ForMember(dest => dest.Completed,
                       opt =>  opt.MapFrom(new XElementTodoItemResolver<bool>("completed")));
            

    }
}