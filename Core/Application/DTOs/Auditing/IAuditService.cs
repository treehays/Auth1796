using Auth1796.Core.Application.Repositories.Common.Interfaces;

namespace Auth1796.Core.Application.DTOs.Auditing;

public interface IAuditService : ITransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(string userId);
}