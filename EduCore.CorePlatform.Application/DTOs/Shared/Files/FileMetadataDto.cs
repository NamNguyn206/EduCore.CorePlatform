using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.DTOs.Shared.Files;

public class FileMetadataDto
{
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public string Extension { get; set; } = string.Empty;
    public Dictionary<string, string> Metadata { get; set; } = new();
    public string? Description { get; set; }
    public bool? IsPublic { get; set; }
}