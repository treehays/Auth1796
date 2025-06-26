using Auth1796.Core.Application.Repositories.Common.Interfaces;

namespace Auth1796.Core.Application.Repositories;

public interface IUnitOfWork : IDisposable, IScopedService
{
    IGenericRepository<TEntity> repository<TEntity>() where TEntity : class;
    Task<bool> Complete();
}