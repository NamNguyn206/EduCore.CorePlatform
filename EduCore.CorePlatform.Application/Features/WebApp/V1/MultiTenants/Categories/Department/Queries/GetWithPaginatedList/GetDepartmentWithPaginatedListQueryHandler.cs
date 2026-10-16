using DocumentFormat.OpenXml.Vml.Office;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Queries.GetWithPaginatedList;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Queries.GetWithPaginatedList;

public sealed class GetDepartmentWithPaginatedListQueryHandler
    : IRequestHandler<GetDepartmentWithPaginatedListQuery, Result<PaginatedResult<DepartmentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetDepartmentWithPaginatedListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<PaginatedResult<DepartmentDto>>> Handle(
        GetDepartmentWithPaginatedListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Department>();

        var listSpec = new DepartmentPaginatedListSpec(_currentTenant, request.RequestData.SearchTerm, request.RequestData.PageIndex, request.RequestData.PageSize);
        var countSpec = new DepartmentCountSpec(_currentTenant, request.RequestData.SearchTerm);

        var items = await repo.ListAsync(listSpec, cancellationToken);
        var total = await repo.CountAsync(countSpec, cancellationToken);

        var result = new PaginatedResult<DepartmentDto>(
            items.Select(x => new DepartmentDto
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address
            }).ToList(),
            total,
            request.RequestData.PageIndex,
            request.RequestData.PageSize);

        return Result<PaginatedResult<DepartmentDto>>.Success(result);
    }
}
