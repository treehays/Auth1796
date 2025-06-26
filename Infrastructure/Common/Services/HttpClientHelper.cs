using Auth1796.Core.Application.DTOs;
using Auth1796.Core.Application.Utilities.Interfaces;
using System.Text;
using System.Text.Json;

namespace Auth1796.Infrastructure.Common.Services;

public class HttpClientHelper : IHttpClientHelper
{
    public async Task<T> MakeRequestAsync<T>(string url, HttpMethod method, object payload = null, List<PostHeaders> headers = null)
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

        using var client = new HttpClient(handler);

        if (headers != null && headers.Count > 0)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var request = new HttpRequestMessage(method, url);

        if (payload != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        }

        var response = await client.SendAsync(request);
        //response.EnsureSuccessStatusCode();

        string responseData = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(responseData, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}