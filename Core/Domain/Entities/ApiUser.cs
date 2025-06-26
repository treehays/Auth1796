namespace Auth1796.Core.Domain.Entities;

public class ApiUser : AuditableEntity
{
    public string? Email { get; set; }
    public string Iv { get; set; }
    public string SecretKey { get; set; }
    public string? ClientId { get; set; }

}