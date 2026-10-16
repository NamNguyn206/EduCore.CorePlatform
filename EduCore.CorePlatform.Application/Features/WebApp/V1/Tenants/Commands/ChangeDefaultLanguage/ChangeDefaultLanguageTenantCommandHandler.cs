using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.Tenants.Commands.ChangeDefaultLanguage;

public sealed class ChangeDefaultLanguageTenantCommandHandler : IRequestHandler<ChangeDefaultLanguageTenantCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public ChangeDefaultLanguageTenantCommandHandler(IUnitOfWork unitOfWork,
                                                ICurrentTenant currentTenant,
                                                ICurrentUser currentUser,
                                                ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(ChangeDefaultLanguageTenantCommand request, CancellationToken cancellationToken)
    {
        var repoTenant = _unitOfWork.Repository<Tenant>();
        var repoLanguage = _unitOfWork.Repository<Language>();

        var specTenant = new TenantByIdForChangeDefaultLanguageSpec(request.RequestData.Id);
        var specLanguage = new LanguageByCodeForChangeTenantDefaultLanguageSpec(request.RequestData.LanguageCode);

        var entityTenant = await repoTenant.FirstOrDefaultAsync(specTenant, cancellationToken);
        var entityLanguage = await repoLanguage.FirstOrDefaultAsync(specLanguage, cancellationToken);

        if (entityTenant == null || entityTenant.IsDeleted)
            return Result.Failure(ErrorCodeConstants.TenantNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.TenantNotFound));

        if (entityLanguage == null || entityLanguage.IsDeleted)
            return Result.Failure(ErrorCodeConstants.LanguageNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.LanguageNotFound));

        entityTenant.ChangeDefaultLanguage(entityLanguage.Id, _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
