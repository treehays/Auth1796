namespace Auth1796.Infrastructure.Common.Middleware;

public class ErrorResult
{
    public string Messages { get; set; } = default;

    public string Source { get; set; }
    public string Exception { get; set; }
    public string ErrorId { get; set; }
    public string SupportMessage { get; set; }
    public int StatusCode { get; set; }
}