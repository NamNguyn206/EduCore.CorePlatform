using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.Tenants.Queries.GetDefaultLanguage;

public sealed class GetDefaultLanguageByTenantIdQueryHandler : IRequestHandler<GetDefaultLanguageByTenantIdQuery, Result<LanguageDto?>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GetDefaultLanguageByTenantIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<LanguageDto?>> Handle(
        GetDefaultLanguageByTenantIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Tenant>();

        var spec = new TenantByIdForDefaultLanguageSpec(request.RequestData.TenantId);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<LanguageDto?>.Failure(ErrorCodeConstants.TenantNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.TenantNotFound));
        }

        LanguageDto? language = null;
        if(entity.DefaultLanguage != null)
        {
            language = new LanguageDto
            {
                Id = entity.DefaultLanguage.Id,
                Code = entity.DefaultLanguage.Code,
                Name = entity.DefaultLanguage.Name
            };
        }

        return Result<LanguageDto?>.Success(language);
    }
}
