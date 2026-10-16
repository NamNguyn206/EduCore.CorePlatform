using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Models;

public class ImportFromExcelRequest
{
    public byte[] FileBytes { get; init; } = default!;
    public string FileName { get; init; } = string.Empty;
}    
