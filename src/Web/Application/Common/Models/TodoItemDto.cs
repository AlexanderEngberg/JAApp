using AutoMapper;
using Web.Domain.Entities;

namespace Web.Application.Common.Models;

public class TodoItemDto
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; }

    public bool Done { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TodoItemDto, TodoItem>();
            CreateMap<TodoItem, TodoItemDto>();
        }
    }
}
