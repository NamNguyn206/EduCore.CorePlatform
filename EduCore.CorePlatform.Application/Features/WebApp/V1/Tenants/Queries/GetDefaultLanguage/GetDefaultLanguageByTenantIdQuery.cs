using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.Tenants.Queries.GetDefaultLanguage;

public record GetDefaultLanguageByTenantIdQuery(GetDefaultLanguageByTenantIdQueryRequest RequestData) : IRequest<Result<LanguageDto?>>;