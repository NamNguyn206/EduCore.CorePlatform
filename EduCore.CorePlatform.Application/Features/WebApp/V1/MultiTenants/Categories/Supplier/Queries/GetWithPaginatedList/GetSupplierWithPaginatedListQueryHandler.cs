using MediatR;
using System;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Queries;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.DTOs;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Queries.GetWithPaginatedList;

public sealed class GetSupplierWithPaginatedListQueryHandler
    : IRequestHandler<GetSupplierWithPaginatedListQuery, Result<PaginatedResult<SupplierDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetSupplierWithPaginatedListQueryHandler(IUnitOfWork unitOfWork, ICurrentTenant tenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = tenant;
    }

    public async Task<Result<PaginatedResult<SupplierDto>>> Handle(
        GetSupplierWithPaginatedListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Supplier>();

        var listSpec = new SupplierPaginatedListWithSupplierAndBankSpec(_currentTenant, request.RequestData.SearchTerm, request.RequestData.PageIndex, request.RequestData.PageSize);
        var countSpec = new SupplierCountSpec(_currentTenant, request.RequestData.SearchTerm);

        var items = await repo.ListAsync(listSpec, cancellationToken);
        var total = await repo.CountAsync(countSpec, cancellationToken);

        var result = new PaginatedResult<SupplierDto>(
            items.Select(x => new SupplierDto
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                TaxCode = x.TaxCode,
                Phone = x.Phone,
                Email = x.Email,
                BankAccount = x.BankAccount,
                Bank = x.Bank != null ? new BankDto() { 
                    Id = x.Bank.Id,
                    Name = x.Bank.Name,
                } : null,
                SupplyCategory = x.SupplyCategory != null ? new SupplyCategoryDto()
                {
                    Id = x.SupplyCategory.Id,
                    Name = x.SupplyCategory.Name,
                } : null,
                Note = x.Note
            }).ToList(),
            total,
            request.RequestData.PageIndex,
            request.RequestData.PageSize);

        return Result<PaginatedResult<SupplierDto>>.Success(result);
    }
}