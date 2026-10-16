using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Commands.Update;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Commands.Update;

public sealed class UpdateBankCommandHandler
    : IRequestHandler<UpdateBankCommand, Result<BankDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public UpdateBankCommandHandler(IUnitOfWork unitOfWork,
                                                ICurrentTenant currentTenant,
                                                ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<BankDto>> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Bank>();

        var spec = new BankByIdForUpdateSpec(_currentTenant, request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null || entity.IsDeleted)
            return Result<BankDto>.Failure(ErrorCodeConstants.BankNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.BankNotFound));

        entity.Update(request.RequestData.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<BankDto>.Success(new BankDto()
        {
            Id = request.RequestData.Id
        });
    }
}
