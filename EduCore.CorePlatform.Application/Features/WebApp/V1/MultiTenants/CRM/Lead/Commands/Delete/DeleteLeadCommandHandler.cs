using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Specifications.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Commands.Delete;

public sealed class DeleteLeadCommandHandler : IRequestHandler<DeleteLeadCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public DeleteLeadCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(DeleteLeadCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.CRM.Lead>();

        var spec = new LeadByIdsForDeleteSpec(_currentTenant, request.RequestData.Ids);

        var entities = await repo.ListAsync(spec, cancellationToken);

        if (entities == null || entities.Count == 0)
            return Result.Failure(ErrorCodeConstants.LeadNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.LeadNotFound));

        if (entities.Count != request.RequestData.Ids.Count)
            return Result.Failure(ErrorCodeConstants.OneOrMoreLeadIdsDoNotExist, await _systemMessageProvider.GetAsync(ErrorCodeConstants.OneOrMoreLeadIdsDoNotExist));

        foreach (var entity in entities)
        {
            entity.SoftDelete(_currentUser.UserId);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}