using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.DTOs.Role;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Queries.GetById;

public sealed class GetTenantUserByIdQueryHandler
    : IRequestHandler<GetTenantUserByIdQuery, Result<TenantUserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GetTenantUserByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<TenantUserDto>> Handle(
        GetTenantUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Identity.User>();

        var spec = new TenantUserByIdSpec(_currentTenant, request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<TenantUserDto>.Failure(ErrorCodeConstants.UserNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserNotFound));
        }

        return Result<TenantUserDto>.Success(new TenantUserDto
        {
            UserId = entity.Id,
            Email = entity.Email,
            FullName = entity.GetFullName(),
            IsTenantAdmin = entity.UserTenants.Select(ut => ut.IsTenantAdmin).FirstOrDefault(),
            Roles = entity.UserTenants.SelectMany(ut => ut.Roles)
                                    .Select(r => new RoleDto
                                    {
                                        Id = r.RoleId,
                                        Name = r.Role.Name
                                    })
                                    .ToList()
        });
    }
}
