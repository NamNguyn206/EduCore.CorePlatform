using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs.Commands;
using EduCore.CorePlatform.Domain.Entities;

namespace EduCore.CorePlatform.Application.Features.Tenants.Commands.Create;

public record CreateTenantCommand(CreateTenantRequest RequestData) : IRequest<Result<TenantDto>>;