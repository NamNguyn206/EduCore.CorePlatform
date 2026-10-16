using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.DTOs.Queries;

public class TenantUserWithPaginatedListQueryRequest : PaginatedListQueryRequest
{
    public Guid? RoleId { get; set; }
}
