using System.ComponentModel;

namespace Auth1796.Core.Application.DTOs;

public record LoginRequest
{
    [DefaultValue("ahmad.abdulsalam@Auth1796.ng")]
    public string? Email { get; set; }
    public SignUpType? LoginType { get; set; }
    public string? Token { get; set; }
    [DefaultValue("P@$$w0rd!123")]
    public string? Password { get; set; }
}

public record LogOutDto
{
    [DefaultValue("ahmad.abdulsalam@Auth1796.ng")]
    public string? Email { get; set; }
}