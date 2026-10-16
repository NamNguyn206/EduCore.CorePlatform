using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Commands.Create;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Commands.Create;

public sealed class CreateBankCommandHandler : IRequestHandler<CreateBankCommand, Result<BankDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public CreateBankCommandHandler(IUnitOfWork unitOfWork,
                                    ICurrentTenant currentTenant,
                                    ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }
    public async Task<Result<BankDto>> Handle(CreateBankCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Bank>();

        var spec = new BankByNameForCreateSpec(_currentTenant, request.RequestData.Name);

        var existing = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (existing != null)
            return Result<BankDto>.Failure(ErrorCodeConstants.BankNameExists, await _systemMessageProvider.GetAsync(ErrorCodeConstants.BankNameExists));

        var entity = new Domain.Entities.MultiTenants.Categories.Bank(_currentTenant.TenantId!.Value, request.RequestData.Name);

        await repo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result<BankDto>.Success(new BankDto()
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}