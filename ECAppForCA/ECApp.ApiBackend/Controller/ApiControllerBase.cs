using ECApp.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECApp.Backend.Controller;

[Authorize]
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ApiControllerBase()
    {
        
    }
    
    protected ICurrentUserService CurrentUserService => HttpContext.RequestServices.GetService<ICurrentUserService>()!;
    
    private ISender? _mediator;

    protected ISender? Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

    protected string? IpAddress => Request.Headers.ContainsKey("X-Forwarded-For")
        ? Request.Headers["X-Forwarded-For"].ToString()
        : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

    protected string? UserAgent => Request.Headers["User-Agent"];
}
