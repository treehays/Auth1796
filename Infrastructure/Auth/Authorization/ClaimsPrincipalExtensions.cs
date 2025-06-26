using Auth1796.Core.Application.Common;
using System.Security.Claims;

namespace Auth1796.Infrastructure.Auth.Authorization;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal principal)
        => principal.FindFirstValue(AppClaims.Email);
    public static string GetRole(this ClaimsPrincipal principal)
        => principal?.FindFirst(AppClaims.Role)?.Value;
    public static string GetPhoneNumber(this ClaimsPrincipal principal)
        => principal.FindFirstValue(AppClaims.PhoneNumber);

    public static string GetUserId(this ClaimsPrincipal principal)
       => principal.FindFirstValue(AppClaims.UserId);
    private static string FindFirstValue(this ClaimsPrincipal principal, string claimType) =>
        principal is null
            ? throw new ArgumentNullException(nameof(principal))
            : principal.FindFirst(claimType)?.Value;
}