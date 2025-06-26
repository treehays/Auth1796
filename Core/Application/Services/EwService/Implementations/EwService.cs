using Auth1796.Core.Application.DTOs;
using Auth1796.Core.Application.Responses;
using Auth1796.Core.Application.Services.EwService.Interfaces;
using Auth1796.Core.Application.Wrapper;
using Microsoft.Extensions.Logging;
using RazorEngineCore;
using System.Text;

namespace Auth1796.Core.Application.Services.EwService.Implementations;
internal sealed class EwService(
    ILogger<EwService> logger) : IEwService
{
    private readonly ILogger<EwService> _logger = logger;

    public async Task<IResult<SendEmailResponse>> SendEmailAsync(MailRequest payload)
    {
        try
        {
            //var sendmailRes = await SendEmail(payload);
            if ("sendmailRes.Value" == "00")
            {
                var sendEmailResponse = new SendEmailResponse
                {
                    Message = "Mail successfully sent",
                    Code = "sendmailRes.Value",
                };
                return await Result<SendEmailResponse>.SuccessAsync(sendEmailResponse);
            }
            return await Result<SendEmailResponse>.FailAsync("We're having trouble sending mail at the moment"); ;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result<SendEmailResponse>.Fail("We're having trouble sending mail at the moment"); ;
        }
    }
    public string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel)
    {
        string template = GetTemplate(templateName);

        IRazorEngine razorEngine = new RazorEngine();
        IRazorEngineCompiledTemplate modifiedTemplate = razorEngine.Compile(template);

        return modifiedTemplate.Run(mailTemplateModel);
    }

    private static string GetTemplate(string templateName)
    {
        //string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var baseDirectory = Directory.GetCurrentDirectory();
        string tmplFolder = Path.Combine(baseDirectory, "EmailTemplates");
        string filePath = Path.Combine(tmplFolder, $"{templateName}.cshtml");
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs, Encoding.Default);
        string mailText = sr.ReadToEnd();
        sr.Close();

        return mailText;
    }
}