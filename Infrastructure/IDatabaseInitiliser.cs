using Auth1796.Core.Application.Repositories.Common.Interfaces;

namespace Auth1796.Infrastructure;

public interface IDatabaseInitiliser : IScopedService
{
    Task SeedDatas();
}