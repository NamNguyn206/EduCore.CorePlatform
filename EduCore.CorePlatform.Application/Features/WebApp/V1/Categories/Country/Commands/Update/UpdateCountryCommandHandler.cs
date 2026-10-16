using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;


namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.Update;

public sealed class UpdateCountryCommandHandler: IRequestHandler<UpdateCountryCommand, Result<CountryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public UpdateCountryCommandHandler(IUnitOfWork unitOfWork,
                                                ICurrentTenant currentTenant,
                                                ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<CountryDto>> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.System.Categories.Country>();

        var spec = new CountryByIdForUpdateSpec(request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null || entity.IsDeleted)
            return Result<CountryDto>.Failure(ErrorCodeConstants.CountryNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.CountryNotFound));

        entity.Update(request.RequestData.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CountryDto>.Success(new CountryDto()
        {
            Id = request.RequestData.Id
        });
    }
}
