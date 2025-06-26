using Auth1796.Core.Application.Repositories;
using Auth1796.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth1796.Infrastructure.Persistence.Repositories;

internal class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class, new()
{
    private readonly ApplicationDbContext _appDbContext;
    public GenericRepository(ApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }


    public void Dispose()
    {
        Dispose(true);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) => await _appDbContext.Set<T>()
        .AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IList<T> entity, CancellationToken cancellationToken) => await _appDbContext.Set<T>()
        .AddRangeAsync(entity, cancellationToken);

    public async Task<T> FindOneAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) =>
        await _appDbContext.Set<T>()
        .FirstOrDefaultAsync(expression, cancellationToken);

    public async Task<IList<T>> GetListAsync(Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default) =>
        await _appDbContext.Set<T>()
        .AsNoTracking()
        .Where(expression)
        .ToListAsync(cancellationToken);

    public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _appDbContext.Entry(entity).State = EntityState.Modified;
        return await _appDbContext.SaveChangesAsync(cancellationToken) != 0;
    }

    public async Task<bool> UpdateRanges(IList<T> entity, CancellationToken cancellationToken = default)
    {
        _appDbContext.Set<T>().UpdateRange(entity);
        return await _appDbContext.SaveChangesAsync(cancellationToken) != 0;
    }

    public async Task<bool> Exist(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) =>
        await _appDbContext.Set<T>().AnyAsync(expression, cancellationToken);

    public async Task<bool> DeleteAsync(string Id, CancellationToken cancellationToken)
    {
        var model = await _appDbContext.Set<T>().FindAsync(Id, cancellationToken);
        if (model is null)
        {
            return false;
        }
        _appDbContext.Set<T>().Remove(model);
        return await _appDbContext.SaveChangesAsync() != 0;
    }


    protected virtual void Dispose(bool isDispose)
    {
        if (isDispose)
        {
            if (_appDbContext is not null)
            {
                _appDbContext.Dispose();
            }
        }
    }
}