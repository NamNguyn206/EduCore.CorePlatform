using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Queries.Search;

public sealed class SearchLanguageQueryHandler
    : IRequestHandler<SearchLanguageQuery, Result<List<LanguageLookupDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public SearchLanguageQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<List<LanguageLookupDto>>> Handle(
        SearchLanguageQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Localization.Language>();
        var spec = new SearchLanguageSpec(
            request.RequestData.SearchTerm, 
            request.RequestData.Limit
        );

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new LanguageLookupDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return Result<List<LanguageLookupDto>>.Success(result);
    }
}
