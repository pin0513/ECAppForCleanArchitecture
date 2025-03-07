using ECApp.Application.Common;
using ECApp.Application.User.Commands;
using ECApp.Application.User.Queries;
using ECApp.Application.User.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECApp.Backend.Controller;

/// <summary>
/// 組織樹 相關接口
/// </summary>
[ApiController]
[Route("api/user")]
public class UserController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpGet("")]
    public async Task<ActionResult> GetUser([FromQuery]  GetUserQuery query)
    {
        var response = await Mediator.Send(query);

        return Ok(Result<UserVm>.Success(response));
    }
    
    [AllowAnonymous]
    [HttpPost("")]
    public async Task<ActionResult> CreateUser([FromQuery]  CreateUserCommand command)
    {
        var response = await Mediator.Send(command);

        return Ok(Result<bool>.Success(response));
    }

}

