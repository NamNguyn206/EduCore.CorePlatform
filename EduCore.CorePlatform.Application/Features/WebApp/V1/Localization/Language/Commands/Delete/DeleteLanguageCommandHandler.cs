using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Commands.Delete;

public sealed class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public DeleteLanguageCommandHandler(IUnitOfWork unitOfWork,
                                        ICurrentUser currentUser,
                                        ICurrentTenant currentTenant,
                                        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Localization.Language>();

        var spec = new LanguageByIdsForDeleteSpec(request.RequestData.Ids);

        var entities = await repo.ListAsync(spec, cancellationToken);

        if (entities == null || entities.Count == 0)
            return Result.Failure(ErrorCodeConstants.LanguageNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.LanguageNotFound));

        if (entities.Count != request.RequestData.Ids.Count)
            return Result.Failure(ErrorCodeConstants.OneOrMoreLanguageIdsDoNotExist, await _systemMessageProvider.GetAsync(ErrorCodeConstants.OneOrMoreLanguageIdsDoNotExist));

        foreach (var entity in entities)
        {
            entity.SoftDelete(_currentUser.UserId);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

