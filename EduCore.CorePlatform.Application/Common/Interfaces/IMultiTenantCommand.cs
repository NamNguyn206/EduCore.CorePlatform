using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Interfaces;

public interface IMultiTenantCommand
{
    Guid? TenantId { get; set; }
}