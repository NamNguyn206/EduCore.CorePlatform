using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.DTOs.Commands;

public class CreateSupplyCategoryRequest
{
    public string Name { get; set; } = default!;
}
