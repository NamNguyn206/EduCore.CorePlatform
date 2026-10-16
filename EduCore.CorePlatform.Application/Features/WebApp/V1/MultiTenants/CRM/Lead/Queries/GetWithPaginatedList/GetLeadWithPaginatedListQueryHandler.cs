using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Specifications.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Queries.GetWithPaginatedList;

public sealed class GetLeadWithPaginatedListQueryHandler
    : IRequestHandler<GetLeadWithPaginatedListQuery, Result<PaginatedResult<LeadDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetLeadWithPaginatedListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<PaginatedResult<LeadDto>>> Handle(
        GetLeadWithPaginatedListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.CRM.Lead>();

        var listSpec = new LeadPaginatedListSpec(
            _currentTenant,
            request.RequestData.SearchTerm,
            request.RequestData.PageIndex,
            request.RequestData.PageSize);

        var countSpec = new LeadCountSpec(
            _currentTenant,
            request.RequestData.SearchTerm);

        var items = await repo.ListAsync(listSpec, cancellationToken);
        var total = await repo.CountAsync(countSpec, cancellationToken);

        var result = new PaginatedResult<LeadDto>(
            items.Select(x => new LeadDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Score = x.Score,
                PredictionPercentage = x.PredictionPercentage,
                Temperature = x.Temperature,
                AiNextAction = x.AiNextAction,
                AssignedUserId = x.AssignedUserId,
                CreatedAt = x.CreatedAt
            }).ToList(),
            total,
            request.RequestData.PageIndex,
            request.RequestData.PageSize);

        return Result<PaginatedResult<LeadDto>>.Success(result);
    }
}