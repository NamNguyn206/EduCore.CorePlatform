using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.DTOs.Commands;

public class UpdateSupplyCategoryRequest : UpdateRequest
{
    public string Name { get; init; } = default!;
}
