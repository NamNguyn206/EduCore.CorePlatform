using EduCore.CorePlatform.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EduCore.CorePlatform.Application.Features.System.SystemMessages.Commands.ImportSystemMessageFromExcel;

public sealed class ImportSystemMessageFromExcelCommand : IRequest<Result>
{
    public byte[] FileBytes { get; init; } = default!;
    public string FileName { get; init; } = string.Empty;
}
