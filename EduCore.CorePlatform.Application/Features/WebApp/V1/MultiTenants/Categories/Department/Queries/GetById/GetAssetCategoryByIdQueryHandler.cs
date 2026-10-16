using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Queries.GetById;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Queries.GetById;

public sealed class GetDepartmentByIdQueryHandler
    : IRequestHandler<GetDepartmentByIdQuery, Result<DepartmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GetDepartmentByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<DepartmentDto>> Handle(
        GetDepartmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Department>();

        var spec = new DepartmentByIdSpec(_currentTenant, request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<DepartmentDto>.Failure(ErrorCodeConstants.DepartmentNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.DepartmentNotFound));
        }

        return Result<DepartmentDto>.Success(new DepartmentDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address
        });
    }
}
