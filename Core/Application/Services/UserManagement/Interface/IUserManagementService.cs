using Auth1796.Core.Application.DTOs.UserManagmentDTO;
using Auth1796.Core.Application.Paging;
using Auth1796.Core.Application.Repositories.Common.Interfaces;
using Auth1796.Core.Application.Wrapper;

namespace Auth1796.Core.Application.Services.UserManagement.Interface;

public interface IUserManagementService : ITransientService
{
    Task<IResult<bool>> RegisterUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
    Task<IResult<PaginatedList<GetUserResponseModel>>> GetUsersAsync(GetUsersRequestModel requestn);
    Task<IResult<IEnumerable<GetRoleResponseModel>>> GetRolesAsync();
    Task<IResult<GetUserResponseModel>> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<IResult<bool>> UpdateUserRole(UpdateUserRoleRequestModel request);
    Task<IResult<bool>> ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

}
