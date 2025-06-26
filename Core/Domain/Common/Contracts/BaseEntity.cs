namespace Auth1796.Core.Domain.Common.Contracts;

public abstract class BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}