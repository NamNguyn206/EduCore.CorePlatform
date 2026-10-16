using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Models;

public sealed class UploadFileRequest
{
    public Stream Stream { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ContentType { get; set; } = default!;
}