using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs.Commands;

namespace EduCore.CorePlatform.Application.Features.Tenants.Commands.Delete;

public sealed record DeleteTenantCommand(DeleteTenantRequest RequestData) : IRequest<Result>;