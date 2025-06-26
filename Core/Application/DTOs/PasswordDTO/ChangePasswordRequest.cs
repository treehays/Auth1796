namespace Auth1796.Core.Application.DTOs.PasswordDTO;

public class ChangePasswordRequest
{
    public string? Password { get; set; } = default!;
    public string? NewPassword { get; set; } = default!;
    public string? ConfirmNewPassword { get; set; } = default!;
}