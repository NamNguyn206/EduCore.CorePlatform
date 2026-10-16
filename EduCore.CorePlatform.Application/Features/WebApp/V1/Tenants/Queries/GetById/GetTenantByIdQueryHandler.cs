using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.Specifications.Queries;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.Tenants.Queries.GetById;

public sealed class GetTenantByIdQueryHandler: IRequestHandler<GetTenantByIdQuery, Result<TenantDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GetTenantByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<TenantDto>> Handle(
        GetTenantByIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Tenant>();

        var spec = new TenantByIdSpec(request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<TenantDto>.Failure(ErrorCodeConstants.TenantNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.TenantNotFound));
        }

        return Result<TenantDto>.Success(new TenantDto
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
