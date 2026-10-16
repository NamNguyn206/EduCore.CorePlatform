using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Queries.GetList;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Queries.Search;

public sealed class SearchManufacturerQueryHandler
    : IRequestHandler<SearchManufacturerQuery, Result<List<ManufacturerLookupDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public SearchManufacturerQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<List<ManufacturerLookupDto>>> Handle(
        SearchManufacturerQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Manufacturer>();
        var spec = new SearchManufacturerSpec(
            _currentTenant, 
            request.RequestData.SearchTerm, 
            request.RequestData.Limit
        );

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new ManufacturerLookupDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return Result<List<ManufacturerLookupDto>>.Success(result);
    }
}
