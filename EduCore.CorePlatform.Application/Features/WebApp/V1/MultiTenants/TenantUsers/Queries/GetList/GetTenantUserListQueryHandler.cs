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

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Queries.GetList;

public sealed class GetTenantUserListQueryHandler
    : IRequestHandler<GetTenantUserListQuery, Result<List<TenantUserDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public GetTenantUserListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<List<TenantUserDto>>> Handle(
        GetTenantUserListQuery request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentTenant.TenantId;
        if (tenantId == null)
        {
            return Result<List<TenantUserDto>>.Failure(ErrorCodeConstants.InvalidTenant, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidTenant));
        }

        var repo = _unitOfWork.Repository<Domain.Entities.Identity.User>();
        var spec = new TenantUserListSpec(
            _currentTenant, 
            request.RequestData.SearchTerm, 
            request.RequestData.RoleId
        );

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new TenantUserDto
        {
            UserId = x.Id,
            Email = x.Email,
            FullName = x.GetFullName(),
            IsTenantAdmin = x.UserTenants.Select(ut => ut.IsTenantAdmin).FirstOrDefault(),
            Roles = x.UserTenants.SelectMany(ut => ut.Roles)
                                    .Select(r => new RoleDto
                                    {
                                        Id = r.RoleId,
                                        Name = r.Role.Name
                                    })
                                    .ToList()
        }).ToList();

        return Result<List<TenantUserDto>>.Success(result);
    }
}
