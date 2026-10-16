using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.DTOs.Commands;

public class CreateDepartmentRequest
{
    public string Name { get; init; } = default!;
    public string Address { get; init; } = default!;
}
