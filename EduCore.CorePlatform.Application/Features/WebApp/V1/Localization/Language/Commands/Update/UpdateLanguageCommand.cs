using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs.Commands;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Commands.Update;

public sealed record UpdateLanguageCommand(UpdateLanguageRequest RequestData) : IRequest<Result<LanguageDto>>;