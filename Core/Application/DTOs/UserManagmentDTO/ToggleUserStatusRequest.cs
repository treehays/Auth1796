namespace Auth1796.Core.Application.DTOs.UserManagmentDTO;

public class ToggleUserStatusRequest
{
    public bool ActivateUser { get; set; }
    public string UserId { get; set; }
}

public record UpdateUserRoleRequestModel
{
    public string? UserId { get; set; }
    public UserRoles RoleName { get; set; }
}