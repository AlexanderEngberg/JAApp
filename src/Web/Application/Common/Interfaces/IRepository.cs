namespace Web.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task AddEntityAsync(T entity, CancellationToken cancellationToken);
    Task UpdateEntityAsync(T entity, CancellationToken cancellationToken);
    Task DeleteEntityAsync(int id, CancellationToken cancellationToken);
}
