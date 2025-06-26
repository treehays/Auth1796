namespace Auth1796.Core.Domain.Common.Contracts;

public abstract class AuditableEntity : BaseEntity, IAuditableEntity, ISoftDelete
{
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; private set; }
    public string LastModifiedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
    public string RemoteIpAddress { get; set; }

    protected AuditableEntity()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}