using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;


namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.Commands.ChangeLanguage;

public sealed class ChangeLanguageUserCommandHandler : IRequestHandler<ChangeLanguageUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public ChangeLanguageUserCommandHandler(IUnitOfWork unitOfWork,
                                                ICurrentTenant currentTenant,
                                                ICurrentUser currentUser,
                                                ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(ChangeLanguageUserCommand request, CancellationToken cancellationToken)
    {
        var repoUser = _unitOfWork.Repository<Domain.Entities.Identity.User>();
        var repoLanguage = _unitOfWork.Repository<Language>();

        var specUser = new UserByIdForChangeLanguageSpec(request.RequestData.Id);
        var specLanguage = new LanguageByCodeForChangeUserLanguageSpec(request.RequestData.LanguageCode);

        var entityUser = await repoUser.FirstOrDefaultAsync(specUser, cancellationToken);
        var entityLanguage = await repoLanguage.FirstOrDefaultAsync(specLanguage, cancellationToken);

        if (entityUser == null || entityUser.IsDeleted)
            return Result.Failure(ErrorCodeConstants.UserNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserNotFound));

        if (entityLanguage == null || entityLanguage.IsDeleted)
            return Result.Failure(ErrorCodeConstants.LanguageNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.LanguageNotFound));

        entityUser.ChangeLanguage(entityLanguage.Id, _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
