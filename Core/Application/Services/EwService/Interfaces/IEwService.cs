using Auth1796.Core.Application.DTOs;
using Auth1796.Core.Application.Repositories.Common.Interfaces;
using Auth1796.Core.Application.Responses;
using Auth1796.Core.Application.Wrapper;

namespace Auth1796.Core.Application.Services.EwService.Interfaces;

internal interface IEwService : IScopedService
{
    Task<IResult<SendEmailResponse>> SendEmailAsync(MailRequest payload);
    string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel);
}