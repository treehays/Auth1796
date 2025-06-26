using Microsoft.AspNetCore.Identity;

namespace Auth1796.Core.Domain.Entities;

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public string CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
}