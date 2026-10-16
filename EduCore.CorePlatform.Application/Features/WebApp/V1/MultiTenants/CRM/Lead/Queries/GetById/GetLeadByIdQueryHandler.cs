using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Specifications.Queries;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Queries.GetById;

public sealed class GetLeadByIdQueryHandler
    : IRequestHandler<GetLeadByIdQuery, Result<LeadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GetLeadByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<LeadDto>> Handle(
        GetLeadByIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.CRM.Lead>();

        var spec = new LeadByIdSpec(_currentTenant, request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<LeadDto>.Failure(ErrorCodeConstants.LeadNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.LeadNotFound));
        }

        return Result<LeadDto>.Success(new LeadDto
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            Score = entity.Score,
            PredictionPercentage = entity.PredictionPercentage,
            Temperature = entity.Temperature,
            AiNextAction = entity.AiNextAction,
            AssignedUserId = entity.AssignedUserId,
            CreatedAt = entity.CreatedAt
        });
    }
}