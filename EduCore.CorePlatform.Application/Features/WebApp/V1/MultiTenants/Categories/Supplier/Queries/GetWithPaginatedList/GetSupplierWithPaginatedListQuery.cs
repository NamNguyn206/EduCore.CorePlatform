using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.DTOs.Queries;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Queries.GetWithPaginatedList;

public sealed record GetSupplierWithPaginatedListQuery(SupplierWithPaginatedListQueryRequest RequestData) : IRequest<Result<PaginatedResult<SupplierDto>>>;