using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Queries.GetWithPaginatedList;

public sealed class GetBankWithPaginatedListQueryHandler
    : IRequestHandler<GetBankWithPaginatedListQuery, Result<PaginatedResult<BankDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    public GetBankWithPaginatedListQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<PaginatedResult<BankDto>>> Handle(
        GetBankWithPaginatedListQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Bank>();

        var listSpec = new BankPaginatedListSpec(_currentTenant, request.RequestData.SearchTerm, request.RequestData.PageIndex, request.RequestData.PageSize);
        var countSpec = new BankCountSpec(_currentTenant, request.RequestData.SearchTerm);

        var items = await repo.ListAsync(listSpec, cancellationToken);
        var total = await repo.CountAsync(countSpec, cancellationToken);

        var result = new PaginatedResult<BankDto>(
            items.Select(x => new BankDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList(),
            total,
            request.RequestData.PageIndex,
            request.RequestData.PageSize);

        return Result<PaginatedResult<BankDto>>.Success(result);
    }
}
