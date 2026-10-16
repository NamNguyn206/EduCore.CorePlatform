using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Models;

public class SearchQueryRequest
{
    public string? SearchTerm { get; set; }
    public int Limit { get; set; } = 10;
}
