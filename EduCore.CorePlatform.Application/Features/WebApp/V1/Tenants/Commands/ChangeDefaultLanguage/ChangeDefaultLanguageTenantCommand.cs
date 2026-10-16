using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs.Commands;

namespace EduCore.CorePlatform.Application.Features.Tenants.Commands.ChangeDefaultLanguage;

public sealed record ChangeDefaultLanguageTenantCommand(ChangeDefaultLanguageTenantRequest RequestData) : IRequest<Result>;