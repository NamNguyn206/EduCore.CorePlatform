using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Queries.GetList;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Queries.Search;

public sealed class SearchSupplierQueryHandler
    : IRequestHandler<SearchSupplierQuery, Result<List<SupplierLookupDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public SearchSupplierQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<List<SupplierLookupDto>>> Handle(
        SearchSupplierQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Supplier>();
        var spec = new SearchSupplierSpec(
            _currentTenant, 
            request.RequestData.SearchTerm, 
            request.RequestData.Limit
        );

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new SupplierLookupDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return Result<List<SupplierLookupDto>>.Success(result);
    }
}
