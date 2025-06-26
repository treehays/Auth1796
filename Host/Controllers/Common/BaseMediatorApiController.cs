using MediatR;

namespace Auth1796.Host.Controllers.Common;

[ApiController]
public class BaseMediatorApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}