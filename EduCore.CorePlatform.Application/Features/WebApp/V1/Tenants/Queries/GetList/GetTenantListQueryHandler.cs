using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.DTOs;
using EduCore.CorePlatform.Application.Features.Tenants.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.Tenants.Queries.GetList;

public sealed class GetTenantListQueryHandler
    : IRequestHandler<GetTenantListQuery, Result<List<TenantDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetTenantListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<List<TenantDto>>> Handle(
        GetTenantListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Tenant>();
        var spec = new TenantListSpec(request.RequestData.SearchTerm);

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new TenantDto
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Description = x.Description,
            Status = x.Status,
            Email = x.Email,
            Phone = x.Phone,
            Address = x.Address,
            Website = x.Website,
            LogoUrl = x.LogoUrl,
            SubscriptionStartDate = x.SubscriptionStartDate,
            SubscriptionEndDate = x.SubscriptionEndDate,
            MaxUsers = x.MaxUsers,
            MaxStorageMB = x.MaxStorageMB,
            TimeZone = x.TimeZone
        }).ToList();

        return Result<List<TenantDto>>.Success(result);
    }
}
