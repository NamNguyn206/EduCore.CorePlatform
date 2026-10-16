using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Commands.Create;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Commands.Create;

public sealed class CreateManufacturerCommandHandler : IRequestHandler<CreateManufacturerCommand, Result<ManufacturerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public CreateManufacturerCommandHandler(IUnitOfWork unitOfWork,
                                    ICurrentTenant currentTenant,
                                    ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }
    public async Task<Result<ManufacturerDto>> Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Manufacturer>();

        var spec = new ManufacturerByNameForCreateSpec(_currentTenant, request.RequestData.Name);

        var existing = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (existing != null)
            return Result<ManufacturerDto>.Failure(ErrorCodeConstants.ManufacturerNameExists, await _systemMessageProvider.GetAsync(ErrorCodeConstants.ManufacturerNameExists));

        var entity = new Domain.Entities.MultiTenants.Categories.Manufacturer(_currentTenant.TenantId!.Value, request.RequestData.Name);

        await repo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result<ManufacturerDto>.Success(new ManufacturerDto()
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}