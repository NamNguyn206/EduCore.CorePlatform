using DocumentFormat.OpenXml.Vml.Office;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Queries.GetList;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Queries.GetList;

public sealed class GetDepartmentListQueryHandler
    : IRequestHandler<GetDepartmentListQuery, Result<List<DepartmentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetDepartmentListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<List<DepartmentDto>>> Handle(
        GetDepartmentListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Department>();
        var spec = new DepartmentListSpec(_currentTenant, request.RequestData.SearchTerm);

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new DepartmentDto
        {
            Id = x.Id,
            Name = x.Name,
            Address = x.Address
        }).ToList();

        return Result<List<DepartmentDto>>.Success(result);
    }
}
