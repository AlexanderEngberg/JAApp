using Web.Application.Common.Models;

namespace Web.Application.Common.Interfaces;

public interface ITodoItemService
{
    Task<int> CreateTodoItem(TodoItemDto dto, CancellationToken cancellationToken);
    Task UpdateTodoItem(TodoItemDto dto, CancellationToken cancellationToken);
    Task DeleteTodoItem(int id, CancellationToken cancellationToken);
}
