using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.Tenants.Queries.GetWithPaginatedList;

public record GetTenantWithPaginatedListQuery(TenantWithPaginatedListQueryRequest RequestData) : IRequest<Result<PaginatedResult<TenantDto>>>;