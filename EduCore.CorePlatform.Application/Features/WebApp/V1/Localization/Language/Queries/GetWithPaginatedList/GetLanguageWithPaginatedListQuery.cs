using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Queries.GetWithPaginatedList;

public record GetLanguageWithPaginatedListQuery(LanguageWithPaginatedListQueryRequest RequestData) : IRequest<Result<PaginatedResult<LanguageDto>>>;