using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.DTOs.Commands;

public class UpdateDepartmentRequest : UpdateRequest
{
    public string Name { get; init; } = default!;
    public string Address { get; set; } = default!;
}
