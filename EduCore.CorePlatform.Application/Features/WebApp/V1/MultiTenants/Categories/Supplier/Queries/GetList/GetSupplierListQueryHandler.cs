using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Queries;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.DTOs;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Queries.GetList;

public sealed class GetSupplierListQueryHandler
    : IRequestHandler<GetSupplierListQuery, Result<List<SupplierDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public GetSupplierListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<List<SupplierDto>>> Handle(
        GetSupplierListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Supplier>();
        var spec = new SupplierListWithSupplierAndBankSpec(_currentTenant, request.RequestData.SearchTerm);

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new SupplierDto
        {
            Id = x.Id,
            Name = x.Name,
            Address = x.Address,
            TaxCode = x.TaxCode,
            Phone = x.Phone,
            Email = x.Email,
            BankAccount = x.BankAccount,
            Bank = x.Bank != null ? new BankDto()
            {
                Id = x.Bank.Id,
                Name = x.Bank.Name,
            } : null,
            SupplyCategory = x.SupplyCategory != null ? new SupplyCategoryDto()
            {
                Id = x.SupplyCategory.Id,
                Name = x.SupplyCategory.Name,
            } : null,
            Note = x.Note
        }).ToList();

        return Result<List<SupplierDto>>.Success(result);
    }
}
