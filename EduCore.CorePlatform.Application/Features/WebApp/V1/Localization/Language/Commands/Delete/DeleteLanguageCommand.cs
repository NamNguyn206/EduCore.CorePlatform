using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs.Commands;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Commands.Delete;

public sealed record DeleteLanguageCommand(DeleteLanguageRequest RequestData) : IRequest<Result>;