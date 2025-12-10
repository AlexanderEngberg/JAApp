using Ardalis.GuardClauses;
using Web.Application.Common.Interfaces;
using Web.Domain.Entities;

namespace Web.Infrastructure.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly IApplicationDbContext _context;

    public TodoItemRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddEntityAsync(TodoItem entity, CancellationToken cancellationToken)
    {
        _context.TodoItems.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateEntityAsync(TodoItem entity, CancellationToken cancellationToken)
    {
        var entityInDb = await _context.TodoItems
            .FindAsync([entity.Id], cancellationToken);

        Guard.Against.NotFound(entity.Id, entityInDb);
        
        entityInDb.Title = entity.Title;
        entityInDb.Done = entity.Done;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteEntityAsync(int id, CancellationToken cancellationToken)
    {
        var entityInDb = await _context.TodoItems
            .FindAsync([id], cancellationToken);

        Guard.Against.NotFound(id, entityInDb);

        _context.TodoItems.Remove(entityInDb);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
