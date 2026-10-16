using EduCore.CorePlatform.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduCore.CorePlatform.API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result)
            : new BadRequestObjectResult(result);
    }
}
