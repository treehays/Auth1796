using Auth1796.Core.Application.DTOs.UserManagmentDTO;
using Auth1796.Core.Application.Paging;
using Auth1796.Core.Application.Repositories.Common.Interfaces;

namespace Auth1796.Core.Application.Repositories;

public interface IUserRepository : ITransientService
{
    Task<PaginatedList<ApplicationUser>> GetUsersAsync(GetUsersRequestModel request);
}