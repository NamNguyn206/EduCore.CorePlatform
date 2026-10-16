using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.Specifications.Queries;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.Tenants.Queries.GetWithPaginatedList;

public sealed class GetTenantWithPaginatedListQueryHandler
    : IRequestHandler<GetTenantWithPaginatedListQuery, Result<PaginatedResult<TenantDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetTenantWithPaginatedListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<PaginatedResult<TenantDto>>> Handle(
        GetTenantWithPaginatedListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Tenant>();

        var listSpec = new TenantPaginatedListSpec(request.RequestData.SearchTerm, request.RequestData.PageIndex, request.RequestData.PageSize);
        var countSpec = new TenantCountSpec(request.RequestData.SearchTerm);

        var items = await repo.ListAsync(listSpec, cancellationToken);
        var total = await repo.CountAsync(countSpec, cancellationToken);

        var result = new PaginatedResult<TenantDto>(
            items.Select(x => new TenantDto
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                Status = x.Status,
                Email = x.Email,
                Phone = x.Phone,
                Address = x.Address,
                Website = x.Website,
                LogoUrl = x.LogoUrl,
                SubscriptionStartDate = x.SubscriptionStartDate,
                SubscriptionEndDate = x.SubscriptionEndDate,
                MaxUsers = x.MaxUsers,
                MaxStorageMB = x.MaxStorageMB,
                TimeZone = x.TimeZone
            }).ToList(),
            total,
            request.RequestData.PageIndex,
            request.RequestData.PageSize);

        return Result<PaginatedResult<TenantDto>>.Success(result);
    }
}
