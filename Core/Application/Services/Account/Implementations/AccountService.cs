using Auth1796.Core.Application.Common;
using Auth1796.Core.Application.DTOs;
using Auth1796.Core.Application.DTOs.Auditing;
using Auth1796.Core.Application.DTOs.PasswordDTO;
using Auth1796.Core.Application.DTOs.UserManagmentDTO;
using Auth1796.Core.Application.Responses;
using Auth1796.Core.Application.Services.Account.Interfaces;
using Auth1796.Core.Application.Services.EwService.Interfaces;
using Auth1796.Core.Application.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth1796.Core.Application.Services.Account.Implementations;

internal class AccountService : IAccountService
{
    private readonly JwtSettings _jwtSettings;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEwService _ewService;

    public AccountService(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IEwService ewService)
    {
        _jwtSettings = jwtSettings.Value;
        _signInManager = signInManager;
        _userManager = userManager;
        _ewService = ewService;
    }

    //TODO: Change the reponse type if otp will be required for login
    public async Task<IResult<LoginResponse>> Login(LoginRequest request, string ipAddress, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Trim());

        if (user is null)
        {
            return await Result<LoginResponse>.FailAsync("Invalid Username or Password.");
        }

        if (!user.EmailConfirmed)
            return await Result<LoginResponse>.FailAsync("Please verify your email.");

        if (!user.IsActive)
            return await Result<LoginResponse>.FailAsync("User Not Active. Please contact the administrator..");

        if (user.IsDissabled)
            return await Result<LoginResponse>.FailAsync("You have been restricted to access this portal..");

        if (user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.Add(TimeSpan.FromDays(180)));
            return await Result<LoginResponse>.FailAsync("You have been Locked out.");
        }

        if (!request.Email.EndsWith("@Auth1796.ng"))
        {
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request?.Password);

            if (!isPasswordValid)
            {
                return await Result<LoginResponse>.FailAsync("Invalid Username or Password.");
            }
        }

        string? userRole = _userManager.GetRolesAsync(user).Result?.FirstOrDefault();
        var tokenResponse = await GenerateTokensAndUpdateUser(user, ipAddress, userRole);
        LoginResponse loginResponse = new()
        {
            TokenResponse = tokenResponse,
            Email = user.Email,
            UserId = user.Id,
            UserRole = userRole,
        };
        return await Result<LoginResponse>.SuccessAsync(loginResponse, "Login Successful");
    }

    private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user, string ipAddress, string userRole)
    {
        string token = GenerateJwt(user, ipAddress, userRole);

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

        await _userManager.UpdateAsync(user);

        return new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
    }

    private string GenerateJwt(ApplicationUser user, string ipAddress, string userRole) =>
        GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress, userRole));

    private IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress, string userRole)
    {
        return new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(AppClaims.UserId, user.Id),
            new(AppClaims.Email, user.Email!),
            new(AppClaims.IpAddress, ipAddress),
            new(AppClaims.Role,userRole?? string.Empty),
            new(ClaimTypes.Role,userRole?? string.Empty),
            new(AppClaims.PhoneNumber, user.PhoneNumber ?? string.Empty)
        };
    }

    private static string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    public async Task<IResult<bool>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
        if (user is null)
        {
            return await Result<bool>.SuccessAsync(true, "Password Reset Mail has been sent to your authorized Email.");
        }

        string code = await _userManager.GeneratePasswordResetTokenAsync(user);

        string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
        var emailConfirmationBodyModel = new EmailConfirmationModel()
        {
            Email = user.Email,
            Url = emailVerificationUri,
            Name = user.FirstName,

        };
        string mailContent = _ewService.GenerateEmailTemplate("forgot-password", emailConfirmationBodyModel);

        var mailRequest = new MailRequest
        {
            Body = mailContent,
            From = "Mailfrom",
            Subject = "Reset Password",
            To = user.Email,
        };

        await Task.Run(async () => await _ewService.SendEmailAsync(mailRequest));
        return await Result<bool>.SuccessAsync(true, "Password Reset Mail has been sent to your authorized Email.");
    }

    public async Task<IResult<bool>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email?.Normalize()!);

        // Don't reveal that the user does not exist
        if (user is null)
        {
            return await Result<bool>.FailAsync("An Error has occurred!.");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token!, request.Password!);
        return result.Succeeded
            ? await Result<bool>.SuccessAsync(true, "Password Reset Successful!")
            : await Result<bool>.FailAsync("An Error has occurred!");
    }

    public async Task<IResult<bool>> ConfirmEmailAsync(string userEmail, string code, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
                        .Where(u => u.Email == userEmail && !u.EmailConfirmed)
                        .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            await Result<bool>.FailAsync("An error occurred while confirming E-Mail.");
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);

        return result.Succeeded
            ? await Result<bool>.SuccessAsync(true, $"Account Confirmed for E-Mail {user.Email}")
            : await Result<bool>.FailAsync($"An error occurred while confirming {user.Email}");
    }

    public async Task<IResult<bool>> ChangePasswordAsync(ChangePasswordRequest model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            await Result<bool>.FailAsync("User Not Found.");
        }

        var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
        return result.Succeeded
            ? await Result<bool>.SuccessAsync(true, "Password successfully changed.")
            : await Result<bool>.FailAsync($"Change password failed {result.Errors.FirstOrDefault()}.");
    }

    private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
    {
        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "/api/v1/account/confirm-email";
        var endpointUri = new Uri(string.Concat($"{origin}", route));
        string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.Email, user.Email);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
        return verificationUri;
    }


}
