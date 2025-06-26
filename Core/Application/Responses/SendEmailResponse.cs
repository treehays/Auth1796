namespace Auth1796.Core.Application.Responses;
public record SendEmailResponse
{
    public string Message { get; set; }
    public string Code { get; set; }
}