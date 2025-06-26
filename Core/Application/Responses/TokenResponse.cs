namespace Auth1796.Core.Application.Responses;

public record TokenResponse(string Token, string RefreshToken, DateTime? RefreshTokenExpiryTime);

public record LoginResponse
{
    public TokenResponse TokenResponse { get; set; }
    public string Email { get; set; }
    public string UserId { get; set; }
    public string UserRole { get; set; }
}