using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Commands.Create;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Commands.Create;

public sealed class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, Result<UnitDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public CreateUnitCommandHandler(IUnitOfWork unitOfWork,
                                    ICurrentTenant currentTenant,
                                    ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }
    public async Task<Result<UnitDto>> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Unit>();

        var spec = new UnitByNameForCreateSpec(_currentTenant, request.RequestData.Name);

        var existing = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (existing != null)
            return Result<UnitDto>.Failure(ErrorCodeConstants.UnitNameExists, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UnitNameExists));

        var entity = new Domain.Entities.MultiTenants.Categories.Unit(_currentTenant.TenantId!.Value, request.RequestData.Name);

        await repo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result<UnitDto>.Success(new UnitDto()
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}