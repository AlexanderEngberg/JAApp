using AutoMapper;
using Web.Application.Common.Interfaces;
using Web.Application.Common.Models;
using Web.Domain.Entities;

namespace Web.Application.Services;

public class TodoItemService : ITodoItemService
{
    private readonly IMapper _mapper;
    private readonly ITodoItemRepository _repository;

    public TodoItemService(ITodoItemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> CreateTodoItem(TodoItemDto dto, CancellationToken cancellationToken)
    {
        TodoItem entity = _mapper.Map<TodoItem>(dto);

        await _repository.AddEntityAsync(entity, cancellationToken);

        return entity.Id;
    }

    public Task UpdateTodoItem(TodoItemDto dto, CancellationToken cancellationToken)
    {
        TodoItem entity = _mapper.Map<TodoItem>(dto);

        return  _repository.UpdateEntityAsync(entity, cancellationToken);
    }

    public Task DeleteTodoItem(int id, CancellationToken cancellationToken)
    {
        return _repository.DeleteEntityAsync(id, cancellationToken);
    }
}
