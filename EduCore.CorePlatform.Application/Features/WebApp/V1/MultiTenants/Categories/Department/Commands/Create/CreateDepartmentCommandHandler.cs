using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Commands.Create;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Commands.Create;

public sealed class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Result<DepartmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public CreateDepartmentCommandHandler(IUnitOfWork unitOfWork,
                                    ICurrentTenant currentTenant,
                                    ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }
    public async Task<Result<DepartmentDto>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Department>();

        var spec = new DepartmentByNameForCreateSpec(
            _currentTenant, 
            request.RequestData.Name,
            request.RequestData.Address
        );

        var existing = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (existing != null)
            return Result<DepartmentDto>.Failure(ErrorCodeConstants.DepartmentNameExists, await _systemMessageProvider.GetAsync(ErrorCodeConstants.DepartmentNameExists));

        var entity = new Domain.Entities.MultiTenants.Categories.Department(
            _currentTenant.TenantId!.Value, 
            request.RequestData.Name,
            request.RequestData.Address
        );

        await repo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result<DepartmentDto>.Success(new DepartmentDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address
        });
    }
}