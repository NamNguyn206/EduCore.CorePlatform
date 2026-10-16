using EduCore.CorePlatform.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.DTOs.Commands;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.Commands.SwitchTenant;

public sealed record SwitchTenantCommand(SwitchTenantRequest RequestData) : IRequest<Result>;