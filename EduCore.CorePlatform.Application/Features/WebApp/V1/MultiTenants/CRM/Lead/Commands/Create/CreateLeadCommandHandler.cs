using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Commands.Create;

public sealed class CreateLeadCommandHandler : IRequestHandler<CreateLeadCommand, Result<LeadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public CreateLeadCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider
    )
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<LeadDto>> Handle(CreateLeadCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.CRM.Lead>();

        var tenantId = _currentTenant.TenantId
            ?? throw new InvalidOperationException("Tenant ID is required to create a Lead.");

        // Khởi tạo entity Lead với các thông tin cơ bản
        var entity = new Domain.Entities.MultiTenants.CRM.Lead(
            request.RequestData.FullName,
            tenantId,
            request.RequestData.PhoneNumber,
            request.RequestData.Email
        );

        // Cập nhật các chỉ số AI Prediction & Score
        entity.UpdateScoreAndPrediction(
            request.RequestData.Score,
            request.RequestData.PredictionPercentage,
            request.RequestData.Temperature
        );

        if (!string.IsNullOrWhiteSpace(request.RequestData.AiNextAction))
        {
            entity.SetAiNextAction(request.RequestData.AiNextAction);
        }

        if (request.RequestData.AssignedUserId.HasValue)
        {
            entity.AssignToUser(request.RequestData.AssignedUserId.Value);
        }

        await repo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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