using Auth1796.Core.Application.Repositories;
using Auth1796.Infrastructure.Persistence.Context;
using System.Collections;

namespace Auth1796.Infrastructure.Persistence.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _appDbContext;
    private Hashtable _repositories;
    public UnitOfWork(ApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<bool> Complete()
    {
        return await _appDbContext.SaveChangesAsync() != 0;
    }

    public void Dispose()
    {
        _appDbContext.Dispose();
    }

    public IGenericRepository<TEntity> repository<TEntity>() where TEntity : class
    {
        if (_repositories == null) _repositories = new Hashtable();
        string Type = typeof(TEntity).Name;
        if (!_repositories.Contains(Type))
        {
            var repositoryType = typeof(GenericRepository<>);
            object repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(TEntity)), _appDbContext);
            _repositories.Add(Type, repositoryInstance);
        }
        return (IGenericRepository<TEntity>)_repositories[Type];
    }
}