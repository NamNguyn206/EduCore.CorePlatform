using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.DTOs.Role;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Queries.GetWithPaginatedList;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Queries.GetWithPaginatedList;

public sealed class GetTenantUserWithPaginatedListQueryHandler
    : IRequestHandler<GetTenantUserWithPaginatedListQuery, Result<PaginatedResult<TenantUserDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public GetTenantUserWithPaginatedListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<PaginatedResult<TenantUserDto>>> Handle(
        GetTenantUserWithPaginatedListQuery request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentTenant.TenantId;

        if (tenantId == null)
        {
            return Result<PaginatedResult<TenantUserDto>>.Failure(ErrorCodeConstants.InvalidTenant, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidTenant));
        }

        var repo = _unitOfWork.Repository<Domain.Entities.Identity.User>();

        var listSpec = new TenantUserPaginatedListSpec(
            _currentTenant, 
            request.RequestData.SearchTerm, 
            request.RequestData.PageIndex, 
            request.RequestData.PageSize,
            request.RequestData.RoleId
        );
        var countSpec = new TenantUserCountSpec(
            _currentTenant, 
            request.RequestData.SearchTerm,
            request.RequestData.RoleId
        );

        var items = await repo.ListAsync(listSpec, cancellationToken);
        var total = await repo.CountAsync(countSpec, cancellationToken);

        var result = new PaginatedResult<TenantUserDto>(
            items.Select(x => new TenantUserDto
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
            }).ToList(),
            total,
            request.RequestData.PageIndex,
            request.RequestData.PageSize);

        return Result<PaginatedResult<TenantUserDto>>.Success(result);
    }
}
