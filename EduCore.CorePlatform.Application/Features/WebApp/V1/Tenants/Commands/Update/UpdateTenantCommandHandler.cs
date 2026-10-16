using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs;
using EduCore.CorePlatform.Application.Features.Tenants.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.Tenants.Commands.Update;

public sealed class UpdateTenantCommandHandler: IRequestHandler<UpdateTenantCommand, Result<TenantDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public UpdateTenantCommandHandler(IUnitOfWork unitOfWork,
                                                ICurrentTenant currentTenant,
                                                ICurrentUser currentUser,
                                                ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<TenantDto>> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Tenant>();

        var spec = new TenantByIdForUpdateSpec(request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null || entity.IsDeleted)
            return Result<TenantDto>.Failure(ErrorCodeConstants.TenantNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.TenantNotFound));

        entity.Update(
            request.RequestData.Name,
            request.RequestData.Description,
            request.RequestData.Email,
            request.RequestData.Phone,
            request.RequestData.Address,
            request.RequestData.Website,
            request.RequestData.SubscriptionStartDate,
            request.RequestData.SubscriptionEndDate,
            request.RequestData.MaxUsers,
            request.RequestData.MaxUsers,
            request.RequestData.TimeZone,
            _currentUser.UserId
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<TenantDto>.Success(new TenantDto()
        {
            Id = request.RequestData.Id
        });
    }
}
