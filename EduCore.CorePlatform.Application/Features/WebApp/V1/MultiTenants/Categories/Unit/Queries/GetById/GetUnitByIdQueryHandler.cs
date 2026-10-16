using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Specifications.Queries;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.DTOs;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Queries.GetById;

public sealed class GetUnitByIdQueryHandler
    : IRequestHandler<GetUnitByIdQuery, Result<UnitDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GetUnitByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<UnitDto>> Handle(
        GetUnitByIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Unit>();

        var spec = new UnitByIdSpec(_currentTenant, request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<UnitDto>.Failure(ErrorCodeConstants.UnitNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UnitNotFound));
        }

        return Result<UnitDto>.Success(new UnitDto
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}
