using Microsoft.AspNetCore.Identity;

namespace Auth1796.Core.Domain.Entities;

public class ApplicationRole : IdentityRole
{
    public string Description { get; set; }

    public ApplicationRole(string name, string description = null)
        : base(name)
    {
        Description = description;
        NormalizedName = name.ToUpperInvariant();
    }
}