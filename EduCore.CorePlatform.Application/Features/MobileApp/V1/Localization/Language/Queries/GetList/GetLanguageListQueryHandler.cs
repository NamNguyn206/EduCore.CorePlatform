using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Localization.Language.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Localization.Language.Queries.GetList;

public sealed class GetLanguageListQueryHandler
    : IRequestHandler<GetLanguageListQuery, Result<List<LanguageDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetLanguageListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<List<LanguageDto>>> Handle(
        GetLanguageListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository < Domain.Entities.Localization.Language>();
        var spec = new LanguageListSpec(request.RequestData.SearchTerm);

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new LanguageDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return Result<List<LanguageDto>>.Success(result);
    }
}
