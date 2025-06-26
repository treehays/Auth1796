using Microsoft.AspNetCore.Identity;

namespace Auth1796.Core.Domain.Entities;

public class ApplicationUser : IdentityUser, IAuditableEntity, ISoftDelete
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Status { get; set; }
    public bool IsActive { get; set; }
    public bool IsDissabled { get; set; }
    public bool IsBackOffice { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string RemoteIpAddress { get; set; } = default!;
}
