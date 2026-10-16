using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.DTOs.Auth;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.DTOs.Commands;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Commands.Create;

public record CreateSupplyCategoryCommand(CreateSupplyCategoryRequest RequestData) : IRequest<Result<SupplyCategoryDto>>;
