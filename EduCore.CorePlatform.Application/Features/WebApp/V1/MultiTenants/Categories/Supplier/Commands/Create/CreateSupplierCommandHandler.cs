using DocumentFormat.OpenXml.Drawing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Commands;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.DTOs;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Commands.Create;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Result<SupplierDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public CreateSupplierCommandHandler(IUnitOfWork unitOfWork, 
                                        ICurrentTenant currentTenant,
                                        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }
    public async Task<Result<SupplierDto>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Supplier>();

        var spec = new SupplierByNameForCreateSpec(_currentTenant, request.RequestData.Code);

        var existing = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (existing != null)
            return Result<SupplierDto>.Failure(ErrorCodeConstants.SupplierCodeExists, await _systemMessageProvider.GetAsync(ErrorCodeConstants.SupplierCodeExists));

        var entity = new Domain.Entities.MultiTenants.Categories.Supplier(
            _currentTenant.TenantId!.Value,
            request.RequestData.Name,
            request.RequestData.Address,
            request.RequestData.TaxCode,            
            request.RequestData.Phone,
            request.RequestData.Email,
            request.RequestData.BankAccount,
            request.RequestData.BankId,
            request.RequestData.BankBranch,
            request.RequestData.SupplyCategoryId,
            request.RequestData.Note            
        );

        await repo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result<SupplierDto>.Success(new SupplierDto() {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}
