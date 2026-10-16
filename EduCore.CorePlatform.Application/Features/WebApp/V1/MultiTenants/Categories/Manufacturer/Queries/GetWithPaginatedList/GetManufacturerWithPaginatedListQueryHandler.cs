using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Specifications.Queries;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.DTOs;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Queries.GetWithPaginatedList;

public sealed class GetManufacturerWithPaginatedListQueryHandler
    : IRequestHandler<GetManufacturerWithPaginatedListQuery, Result<PaginatedResult<ManufacturerDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetManufacturerWithPaginatedListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<PaginatedResult<ManufacturerDto>>> Handle(
        GetManufacturerWithPaginatedListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Manufacturer>();

        var listSpec = new ManufacturerPaginatedListSpec(_currentTenant, request.RequestData.SearchTerm, request.RequestData.PageIndex, request.RequestData.PageSize);
        var countSpec = new ManufacturerCountSpec(_currentTenant, request.RequestData.SearchTerm);

        var items = await repo.ListAsync(listSpec, cancellationToken);
        var total = await repo.CountAsync(countSpec, cancellationToken);

        var result = new PaginatedResult<ManufacturerDto>(
            items.Select(x => new ManufacturerDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList(),
            total,
            request.RequestData.PageIndex,
            request.RequestData.PageSize);

        return Result<PaginatedResult<ManufacturerDto>>.Success(result);
    }
}
