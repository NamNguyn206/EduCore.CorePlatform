using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Queries.GetLanguage;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Commands.ChangeLanguage;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Commands.SwitchTenant;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.DTOs.Commands;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.DTOs.Queries;
using EduCore.CorePlatform.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduCore.CorePlatform.API.Controllers.MobileApp.V1.Identities;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/mobile-app/v{version:apiVersion}/identities/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public UsersController(IMediator mediator,
                            ISystemMessageProvider systemMessageProvider)
    {
        _mediator = mediator;
        _systemMessageProvider = systemMessageProvider;
    }
    [HttpGet("settings/get-language")]
    [ProducesResponseType(typeof(Result<UserLanguageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLanguage([FromBody] GetLanguageByUserIdQueryRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new GetLanguageByUserIdQuery(request));
            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("settings/change-language")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeLanguage([FromBody] ChangeLanguageUserRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new ChangeLanguageUserCommand(request));
            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("settings/switch-tenant")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SwitchTenant([FromBody] SwitchTenantRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new SwitchTenantCommand(request));
            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
