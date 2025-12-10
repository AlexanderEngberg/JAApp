using Microsoft.EntityFrameworkCore;
using Web.Domain.Entities;

namespace Web.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
