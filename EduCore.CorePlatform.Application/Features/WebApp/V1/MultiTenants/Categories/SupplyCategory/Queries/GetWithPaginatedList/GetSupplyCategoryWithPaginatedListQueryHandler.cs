using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Queries.GetWithPaginatedList;

public sealed class GetSupplyCategoryWithPaginatedListQueryHandler
    : IRequestHandler<GetSupplyCategoryWithPaginatedListQuery, Result<PaginatedResult<SupplyCategoryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetSupplyCategoryWithPaginatedListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<PaginatedResult<SupplyCategoryDto>>> Handle(
        GetSupplyCategoryWithPaginatedListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.SupplyCategory>();

        var listSpec = new SupplyCategoryPaginatedListSpec(_currentTenant, request.RequestData.SearchTerm, request.RequestData.PageIndex, request.RequestData.PageSize);
        var countSpec = new SupplyCategoryCountSpec(_currentTenant, request.RequestData.SearchTerm);

        var items = await repo.ListAsync(listSpec, cancellationToken);
        var total = await repo.CountAsync(countSpec, cancellationToken);

        var result = new PaginatedResult<SupplyCategoryDto>(
            items.Select(x => new SupplyCategoryDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList(),
            total,
            request.RequestData.PageIndex,
            request.RequestData.PageSize);

        return Result<PaginatedResult<SupplyCategoryDto>>.Success(result);
    }
}
