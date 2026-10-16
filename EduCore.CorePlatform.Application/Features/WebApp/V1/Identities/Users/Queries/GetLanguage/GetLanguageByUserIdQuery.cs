using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.Queries.GetLanguage;

public record GetLanguageByUserIdQuery(GetLanguageByUserIdQueryRequest RequestData) : IRequest<Result<LanguageDto?>>;