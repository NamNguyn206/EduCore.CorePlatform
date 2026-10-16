using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.DTOs.Queries;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.DTOs;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Queries.GetWithPaginatedList;

public record GetTenantUserWithPaginatedListQuery(TenantUserWithPaginatedListQueryRequest RequestData) : IRequest<Result<PaginatedResult<TenantUserDto>>>;