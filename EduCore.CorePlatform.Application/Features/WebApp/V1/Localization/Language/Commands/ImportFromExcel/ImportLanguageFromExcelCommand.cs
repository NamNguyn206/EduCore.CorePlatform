using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs.Commands;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Commands.ImportFromExcel;

public sealed record ImportLanguageFromExcelCommand(ImportLanguageFromExcelRequest RequestData) : IRequest<Result>;