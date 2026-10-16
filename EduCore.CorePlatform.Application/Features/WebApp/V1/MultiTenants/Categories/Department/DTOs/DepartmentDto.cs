using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.DTOs;

public class DepartmentDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
}
