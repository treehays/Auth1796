namespace Auth1796.Core.Application.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IList<T> entity, CancellationToken cancellationToken = default);
    Task<T> FindOneAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    Task<IList<T>> GetListAsync(Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateRanges(IList<T> entity, CancellationToken cancellationToken = default);
    Task<bool> Exist(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string Id, CancellationToken cancellationToken);
}