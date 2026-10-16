using EduCore.CorePlatform.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Queries.GetLanguage;

public record GetLanguageByUserIdQuery(GetLanguageByUserIdQueryRequest RequestData) : IRequest<Result<UserLanguageDto?>>;