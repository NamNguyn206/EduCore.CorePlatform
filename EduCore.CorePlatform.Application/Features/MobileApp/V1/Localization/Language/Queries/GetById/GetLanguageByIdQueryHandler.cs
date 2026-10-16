using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Localization.Language.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Localization.Language.Queries.GetById;

public sealed class GetLanguageByIdQueryHandler
    : IRequestHandler<GetLanguageByIdQuery, Result<LanguageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GetLanguageByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<LanguageDto>> Handle(
        GetLanguageByIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Localization.Language>();

        var spec = new LanguageByIdSpec(request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<LanguageDto>.Failure(ErrorCodeConstants.LanguageNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.LanguageNotFound));
        }

        return Result<LanguageDto>.Success(new LanguageDto
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}
