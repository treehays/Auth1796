using System.Security.Claims;

namespace Auth1796.Core.Application.Utilities.Interfaces;

public interface ICurrentUser
{
    string Name { get; }

    string GetUserId();

    string GetUserEmail();
    string GetUserRole();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim> GetUserClaims();
}