using Auth1796.Core.Application.DTOs.UserManagmentDTO;
using Auth1796.Core.Application.Paging;
using Auth1796.Core.Application.Repositories;
using Auth1796.Core.Application.Services.UserManagement.Interface;
using Auth1796.Core.Application.Utilities;
using Auth1796.Core.Application.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth1796.Core.Application.Services.UserManagement.Implementation;
internal class UserManagementService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserManagementService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IResult<bool>> RegisterUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userExist = await _unitOfWork.repository<ApplicationUser>().Exist(x => x.Email == request.Email, cancellationToken);
        if (userExist)
        {
            return await Result<bool>.FailAsync($"User already exist.");
        }

        var role = await _roleManager.FindByNameAsync(request.RoleName);
        if (role == null)
        {
            return await Result<bool>.FailAsync($"select a valid user role.");
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
            IsActive = true,
            EmailConfirmed = true,
            IsBackOffice = true,
            PhoneNumberConfirmed = true,
            Status = Status.Active.ToFriendlyString(),
        };

        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            return await Result<bool>.FailAsync($"Validation Errors Occurred {result.Errors.FirstOrDefault()}.");
        }

        await _userManager.AddToRoleAsync(user, role.Name);
        return await Result<bool>.SuccessAsync(true, $"{role.Name} user added succeddfully.");
    }

    public async Task<IResult<GetUserResponseModel>> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.repository<ApplicationUser>()
                                    .FindOneAsync(x => x.Id == userId, cancellationToken);
        if (user is null)
        {
            return await Result<GetUserResponseModel>.FailAsync($"user not found");
        }

        var response = new GetUserResponseModel
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            CreatedOn = user.CreatedAt,
            IsActive = user.IsActive,
            UserRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault().ToEnum<UserRoles>()
        };

        return await Result<GetUserResponseModel>.SuccessAsync(response);
    }

    public async Task<IResult<IEnumerable<GetRoleResponseModel>>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var roleModel = roles.Select(x => new GetRoleResponseModel
        {
            Id = x.Id,
            Name = x.Name,
        });
        return await Result<IEnumerable<GetRoleResponseModel>>.SuccessAsync(roleModel);
    }

    public async Task<IResult<PaginatedList<GetUserResponseModel>>> GetUsersAsync(GetUsersRequestModel request)
    {
        var users = await _userRepository.GetUsersAsync(request);

        var userDto = users.Items.
                           Select(user => new GetUserResponseModel
                           {
                               Id = user.Id,
                               Email = user.Email,
                               FirstName = user.FirstName,
                               LastName = user.LastName,
                               PhoneNumber = user.PhoneNumber,
                               CreatedOn = user.CreatedAt,
                               IsActive = user.IsActive,
                               UserRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault().ToEnum<UserRoles>()
                           })
                           .ToList();

        var response = new PaginatedList<GetUserResponseModel>
        {
            Items = userDto,
            PageSize = users.PageSize,
            TotalItems = users.TotalItems,
            Page = users.Page
        };

        return await Result<PaginatedList<GetUserResponseModel>>.SuccessAsync(response);
    }

    public async Task<IResult<bool>> UpdateUserRole(UpdateUserRoleRequestModel request)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            return await Result<bool>.FailAsync("Invalid input..");

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return await Result<bool>.FailAsync("User not found..");

        var roles = await _userManager.GetRolesAsync(user);
        if (roles is null)
        {
            await _userManager.AddToRoleAsync(user, request.RoleName.ToFriendlyString());
            return await Result<bool>.SuccessAsync(true, "role successfully assigned.");
        }

        await _userManager.RemoveFromRolesAsync(user, roles);
        await _userManager.AddToRoleAsync(user, request.RoleName.ToFriendlyString());
        return await Result<bool>.SuccessAsync(true, "role successfully updated..");
    }

    public async Task<IResult<bool>> ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return await Result<bool>.FailAsync($"user not found");
        }

        bool isSuperAdmin = await _userManager.IsInRoleAsync(user, UserRoles.BackOfficeSuperAdmin.ToFriendlyString());
        if (isSuperAdmin)
        {
            throw new ConflictException("Administrators Profile's Status cannot be toggled");
        }

        user.IsActive = request.ActivateUser;
        await _userManager.UpdateAsync(user);
        return await Result<bool>.SuccessAsync(true, $"user status updated.");
    }

}