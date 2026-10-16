using EduCore.CorePlatform.API.Extensions;
using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Commands.Create;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Commands.Delete;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Commands.ImportFromExcel;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Commands.Update;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs.Commands;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs.Queries;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Queries.ExportToExcel;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Queries.GetById;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Queries.GetList;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Queries.GetWithPaginatedList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduCore.CorePlatform.API.Controllers.WebApp.V1.MultiTenants.CRM;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/web-app/v{version:apiVersion}/multi-tenants/crm/leads")]
public class LeadsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public LeadsController(IMediator mediator,
        ISystemMessageProvider systemMessageProvider)
    {
        _mediator = mediator;
        _systemMessageProvider = systemMessageProvider;
    }
    [HttpPost("create")]
    [ProducesResponseType(typeof(Result<LeadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateLeadRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateLeadCommand(request));
            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromBody] DeleteLeadRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new DeleteLeadCommand(request));

            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("import-from-excel")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportFromExcel(IFormFile file)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var request = new ImportLeadFromExcelRequest()
            {
                FileBytes = fileBytes,
                FileName = file.FileName
            };

            var command = new ImportLeadFromExcelCommand(request);

            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update/{id:guid}")]
    [ProducesResponseType(typeof(Result<LeadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateLeadRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.Id = id;

            var result = await _mediator.Send(new UpdateLeadCommand(request));
            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("export-to-excel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExportToExcel([FromQuery] ExportLeadToExcelQueryRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new ExportLeadToExcelQuery(request));

            if (!result.IsSuccess)
                return result.ToActionResult();

            var fileBytes = result.Data;

            var fileName = $"leads_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-by-id/{id:guid}")]
    [ProducesResponseType(typeof(Result<LeadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById([FromRoute] LeadByIdQueryRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new GetLeadByIdQuery(request));
            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-list")]
    [ProducesResponseType(typeof(Result<List<LeadDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetList([FromQuery] LeadListQueryRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new GetLeadListQuery(request));
            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-with-paginated-list")]
    [ProducesResponseType(typeof(Result<PaginatedResult<LeadDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetWithPaginatedList([FromQuery] LeadWithPaginatedListQueryRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate paging
            if (request.PageIndex <= 0)
                request.PageIndex = 1;

            if (request.PageSize <= 0)
                request.PageSize = 10;

            var result = await _mediator.Send(new GetLeadWithPaginatedListQuery(request));

            return result.ToActionResult();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
