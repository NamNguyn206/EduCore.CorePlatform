using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Commands;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Commands.Update;

public sealed class UpdateSupplierCommandHandler
    : IRequestHandler<UpdateSupplierCommand, Result<SupplierDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public UpdateSupplierCommandHandler(IUnitOfWork unitOfWork,
                                        ICurrentTenant currentTenant,
                                        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<SupplierDto>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Supplier>();

        var spec = new SupplierByIdForUpdateSpec(_currentTenant, request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null || entity.IsDeleted)
            return Result<SupplierDto>.Failure(ErrorCodeConstants.SupplierNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.SupplierNotFound));

        entity.Update(
            request.RequestData.Name,
            request.RequestData.Address,
            request.RequestData.TaxCode,            
            request.RequestData.Phone,
            request.RequestData.Email,
            request.RequestData.BankAccount,
            request.RequestData.BankId,
            request.RequestData.BankBranch,
            request.RequestData.SupplyCategoryId,
            request.RequestData.Note);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<SupplierDto>.Success(new SupplierDto()
        {
            Id = request.RequestData.Id
        });
    }
}
