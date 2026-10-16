using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Queries.GetById;

public sealed class GetSupplyCategoryByIdQueryHandler
    : IRequestHandler<GetSupplyCategoryByIdQuery, Result<SupplyCategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GetSupplyCategoryByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<SupplyCategoryDto>> Handle(
        GetSupplyCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.SupplyCategory>();

        var spec = new SupplyCategoryByIdSpec(_currentTenant, request.RequestData.Id);
        
        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<SupplyCategoryDto>.Failure(ErrorCodeConstants.SupplyCategoryNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.SupplyCategoryNotFound));
        }

        return Result<SupplyCategoryDto>.Success(new SupplyCategoryDto
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}
