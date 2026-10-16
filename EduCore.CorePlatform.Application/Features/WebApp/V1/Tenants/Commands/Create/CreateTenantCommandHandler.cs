using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.Commands.Create;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs;
using EduCore.CorePlatform.Application.Features.Tenants.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Domain.Common.Enums;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.Tenants.Commands.Create;

public sealed class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Result<TenantDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public CreateTenantCommandHandler(IUnitOfWork unitOfWork,
                                    ICurrentUser currentUser,
                                    ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _systemMessageProvider = systemMessageProvider;
    }
    public async Task<Result<TenantDto>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Tenant>();

        var spec = new TenantByCodeForCreateSpec(request.RequestData.Code);

        var existing = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (existing != null)
            return Result<TenantDto>.Failure(ErrorCodeConstants.TenantAlreadyExists, await _systemMessageProvider.GetAsync(ErrorCodeConstants.TenantAlreadyExists));

        var entity = new Tenant(
            request.RequestData.Code,
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

        entity.Activate();

        await repo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result<TenantDto>.Success(new TenantDto()
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            Status = entity.Status,
            Email = entity.Email,
            Phone = entity.Phone,
            Address = entity.Address,
            Website = entity.Website,
            LogoUrl = entity.LogoUrl,
            SubscriptionStartDate = entity.SubscriptionStartDate,
            SubscriptionEndDate = entity.SubscriptionEndDate,
            MaxUsers = entity.MaxUsers,
            MaxStorageMB = entity.MaxStorageMB,
            TimeZone = entity.TimeZone
        });
    }
}