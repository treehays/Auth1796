namespace Auth1796.Core.Application.DTOs;

public class MailRequest
{
    public string? To { get; set; }
    public string? From { get; set; }
    public string? Body { get; set; }
    public string? Subject { get; set; }
}
