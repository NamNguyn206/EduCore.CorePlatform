using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Queries.GetList;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Queries.Search;

public sealed class SearchBankQueryHandler
    : IRequestHandler<SearchBankQuery, Result<List<BankLookupDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;

    public SearchBankQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
    }

    public async Task<Result<List<BankLookupDto>>> Handle(
        SearchBankQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Bank>();
        var spec = new SearchBankSpec(
            _currentTenant, 
            request.RequestData.SearchTerm, 
            request.RequestData.Limit
        );

        var data = await repo.ListAsync(spec, cancellationToken);

        var result = data.Select(x => new BankLookupDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return Result<List<BankLookupDto>>.Success(result);
    }
}
