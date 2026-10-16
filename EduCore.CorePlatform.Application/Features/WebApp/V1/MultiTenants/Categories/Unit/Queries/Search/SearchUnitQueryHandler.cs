using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Queries.GetList;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Queries.Search;

public sealed class SearchUnitQueryHandler
    : IRequestHandler<SearchUnitQuery, Result<List<UnitLookupDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public SearchUnitQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<List<UnitLookupDto>>> Handle(
        SearchUnitQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Unit>();
        var spec = new SearchUnitSpec(
            _currentTenant, 
            request.RequestData.SearchTerm, 
            request.RequestData.Limit
        );

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new UnitLookupDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return Result<List<UnitLookupDto>>.Success(result);
    }
}
