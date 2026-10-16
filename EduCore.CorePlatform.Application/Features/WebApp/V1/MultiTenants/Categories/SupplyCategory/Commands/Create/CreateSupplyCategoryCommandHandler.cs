using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Commands.Create;

public sealed class CreateSupplyCategoryCommandHandler : IRequestHandler<CreateSupplyCategoryCommand, Result<SupplyCategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public CreateSupplyCategoryCommandHandler(IUnitOfWork unitOfWork,
                                            ICurrentTenant currentTenant,
                                            ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }
    public async Task<Result<SupplyCategoryDto>> Handle(CreateSupplyCategoryCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.SupplyCategory>();

        var spec = new SupplyCategoryByNameForCreateSpec(_currentTenant, request.RequestData.Name);

        var existing = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (existing != null)
            return Result<SupplyCategoryDto>.Failure(ErrorCodeConstants.SupplyCategoryNameExists, await _systemMessageProvider.GetAsync(ErrorCodeConstants.SupplyCategoryNameExists));

        var entity = new Domain.Entities.MultiTenants.Categories.SupplyCategory(_currentTenant.TenantId!.Value, request.RequestData.Name);

        await repo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result<SupplyCategoryDto>.Success(new SupplyCategoryDto() { 
            Id = entity.Id,
            Name = entity.Name
        });
    }
}
