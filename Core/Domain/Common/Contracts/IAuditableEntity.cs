namespace Auth1796.Core.Domain.Common.Contracts;

public interface IAuditableEntity
{
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; }
    public string LastModifiedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string RemoteIpAddress { get; set; }
}