using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.DTOs.Identity;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.DTOs.Commands;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.DTOs;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Commands.Create;

public sealed record CreateTenantUserCommand(CreateTenantUserRequest RequestData): IRequest<Result<TenantUserDto>>;