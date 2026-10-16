using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Queries.GetWithPaginatedList;

public sealed class GetLanguageWithPaginatedListQueryHandler
    : IRequestHandler<GetLanguageWithPaginatedListQuery, Result<PaginatedResult<LanguageDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetLanguageWithPaginatedListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<PaginatedResult<LanguageDto>>> Handle(
        GetLanguageWithPaginatedListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Localization.Language>();

        var listSpec = new LanguagePaginatedListSpec(request.RequestData.SearchTerm, request.RequestData.PageIndex, request.RequestData.PageSize);
        var countSpec = new LanguageCountSpec(request.RequestData.SearchTerm);

        var items = await repo.ListAsync(listSpec, cancellationToken);
        var total = await repo.CountAsync(countSpec, cancellationToken);

        var result = new PaginatedResult<LanguageDto>(
            items.Select(x => new LanguageDto
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            }).ToList(),
            total,
            request.RequestData.PageIndex,
            request.RequestData.PageSize);

        return Result<PaginatedResult<LanguageDto>>.Success(result);
    }
}
