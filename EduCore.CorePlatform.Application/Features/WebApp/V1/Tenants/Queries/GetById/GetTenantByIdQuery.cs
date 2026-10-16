using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs.Queries;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs;

namespace EduCore.CorePlatform.Application.Features.Tenants.Queries.GetById;

public record GetTenantByIdQuery(TenantByIdQueryRequest RequestData) : IRequest<Result<TenantDto>>;