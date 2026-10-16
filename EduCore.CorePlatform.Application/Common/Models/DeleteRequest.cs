using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Models;

public class DeleteRequest
{
    public List<Guid> Ids { get; init; } = default!;
}
