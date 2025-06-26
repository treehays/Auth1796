using Auth1796.Core.Application.DTOs;
using Auth1796.Core.Application.DTOs.PasswordDTO;
using Auth1796.Core.Application.Repositories.Common.Interfaces;
using Auth1796.Core.Application.Responses;
using Auth1796.Core.Application.Wrapper;

namespace Auth1796.Core.Application.Services.Account.Interfaces;

public interface IAccountService : IScopedService
{
    Task<IResult<LoginResponse>> Login(LoginRequest request, string remoteIp, CancellationToken cancellationToken);
    Task<IResult<bool>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
    Task<IResult<bool>> ConfirmEmailAsync(string userEmail, string code, CancellationToken cancellationToken);
    Task<IResult<bool>> ChangePasswordAsync(ChangePasswordRequest model, string userId);
    Task<IResult<bool>> ResetPasswordAsync(ResetPasswordRequest request);
}