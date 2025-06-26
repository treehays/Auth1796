using Auth1796.Core.Application.Paging;

namespace Auth1796.Core.Application.DTOs.UserManagmentDTO;
public class CreateUserRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string RoleName { get; set; }
    public string? PhoneNumber { get; set; }
}

public class GetUserResponseModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public UserRoles UserRole { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedOn { get; set; }
}

public record GetUsersRequestModel : PageRequest
{
    public UserRoles? RoleName { get; set; }
}