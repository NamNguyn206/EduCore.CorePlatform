using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Commands.Update;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Specifications.Commands;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Commands.Update;

public sealed class UpdateLeadCommandHandler
    : IRequestHandler<UpdateLeadCommand, Result<LeadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public UpdateLeadCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<LeadDto>> Handle(UpdateLeadCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<EduCore.CorePlatform.Domain.Entities.MultiTenants.CRM.Lead>();

        var spec = new LeadByIdForUpdateSpec(_currentTenant, request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null || entity.IsDeleted)
            return Result<LeadDto>.Failure(ErrorCodeConstants.LeadNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.LeadNotFound));

        // Cập nhật các thông tin của Lead
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

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<LeadDto>.Success(new LeadDto
        {
            Id = request.RequestData.Id,
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