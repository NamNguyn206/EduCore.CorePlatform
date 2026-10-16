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
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Queries.GetById;

public sealed class GetSupplierByIdQueryHandler
    : IRequestHandler<GetSupplierByIdQuery, Result<SupplierDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GetSupplierByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<SupplierDto>> Handle(
        GetSupplierByIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Supplier>();

        var spec = new SupplierByIdWithSupplierAndBankSpec(_currentTenant, request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<SupplierDto>.Failure(ErrorCodeConstants.SupplierNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.SupplierNotFound));
        }

        return Result<SupplierDto>.Success(new SupplierDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
            TaxCode = entity.TaxCode,
            Phone = entity.Phone,
            Email = entity.Email,
            BankAccount = entity.BankAccount,
            Bank = entity.Bank != null ? new BankDto()
            {
                Id = entity.Bank.Id,
                Name = entity.Bank.Name,
            } : null,
            SupplyCategory = entity.SupplyCategory != null ? new SupplyCategoryDto()
            {
                Id = entity.SupplyCategory.Id,
                Name = entity.SupplyCategory.Name,
            } : null,
            Note = entity.Note
        });
    }
}
