using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.DTOs.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Commands.SwitchTenant;

public sealed record SwitchTenantCommand(SwitchTenantRequest RequestData) : IRequest<Result>;