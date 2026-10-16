using EduCore.CorePlatform.API.Extensions;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Login.Commands;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Logout.Commands;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.RefreshToken.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCore.CorePlatform.API.Controllers.MobileApp.V1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/mobile-app/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Result<CorePlatform.Application.Features.MobileApp.V1.Auth.Login.DTOs.LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] CorePlatform.Application.Features.MobileApp.V1.Auth.Login.DTOs.LoginRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new LoginCommand(request));

            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(Result<CorePlatform.Application.Features.MobileApp.V1.Auth.RefreshToken.DTOs.LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] CorePlatform.Application.Features.MobileApp.V1.Auth.RefreshToken.DTOs.RefreshTokenRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new RefreshTokenCommand(request));

            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("logout")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout([FromBody] CorePlatform.Application.Features.MobileApp.V1.Auth.Logout.DTOs.LogoutRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new LogoutCommand(request));

            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
