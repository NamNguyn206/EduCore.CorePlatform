using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.Persistence;


namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Queries.Search;

public sealed class SearchCountryQueryHandler
    : IRequestHandler<SearchCountryQuery, Result<List<CountryLookupDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public SearchCountryQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<List<CountryLookupDto>>> Handle(
        SearchCountryQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.System.Categories.Country>();
        var spec = new SearchCountrySpec(
            request.RequestData.SearchTerm, 
            request.RequestData.Limit
        );

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new CountryLookupDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return Result<List<CountryLookupDto>>.Success(result);
    }
}
