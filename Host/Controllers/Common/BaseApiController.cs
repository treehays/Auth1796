using Auth1796.Core.Application.Wrapper;

namespace Auth1796.Host.Controllers.Common;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<dynamic>))]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<dynamic>))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<dynamic>))]
public partial class BaseApiController : ControllerBase
{
    protected string GetIpAddress() =>
     Request.Headers.ContainsKey("X-Forwarded-For")
         ? Request.Headers["X-Forwarded-For"]
         : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
}
