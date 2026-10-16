using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Queries;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;
using EduCore.CorePlatform.Application.Abstractions.Persistence;


namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Queries.GetList;

public sealed class GetCountryListQueryHandler
    : IRequestHandler<GetCountryListQuery, Result<List<CountryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetCountryListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<List<CountryDto>>> Handle(
        GetCountryListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.System.Categories.Country>();
        var spec = new CountryListSpec(request.RequestData.SearchTerm);

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new CountryDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return Result<List<CountryDto>>.Success(result);
    }
}
