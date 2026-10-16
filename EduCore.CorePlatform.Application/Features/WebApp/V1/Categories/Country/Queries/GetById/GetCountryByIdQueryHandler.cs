using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Queries;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;


namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Queries.GetById;

public sealed class GetCountryByIdQueryHandler
    : IRequestHandler<GetCountryByIdQuery, Result<CountryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GetCountryByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<CountryDto>> Handle(
        GetCountryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.System.Categories.Country>();

        var spec = new CountryByIdSpec(request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<CountryDto>.Failure(ErrorCodeConstants.CountryNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.CountryNotFound));
        }

        return Result<CountryDto>.Success(new CountryDto
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}
