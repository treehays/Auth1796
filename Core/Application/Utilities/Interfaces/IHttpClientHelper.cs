using Auth1796.Core.Application.DTOs;
using Auth1796.Core.Application.Repositories.Common.Interfaces;

namespace Auth1796.Core.Application.Utilities.Interfaces;

public interface IHttpClientHelper : IScopedService
{
    Task<T> MakeRequestAsync<T>(string url, HttpMethod method, object payload = null, List<PostHeaders> headers = null);
}