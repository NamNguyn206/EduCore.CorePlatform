using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.Create;

public sealed class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Result<CountryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public CreateCountryCommandHandler(IUnitOfWork unitOfWork,
                                    ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _systemMessageProvider = systemMessageProvider;
    }
    public async Task<Result<CountryDto>> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.System.Categories.Country>();

        var spec = new CountryByNameForCreateSpec(request.RequestData.Name);

        var existing = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (existing != null)
            return Result<CountryDto>.Failure(ErrorCodeConstants.CountryNameExists, await _systemMessageProvider.GetAsync(ErrorCodeConstants.CountryNameExists));

        var entity = new Domain.Entities.System.Categories.Country(request.RequestData.Name);

        await repo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result<CountryDto>.Success(new CountryDto()
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}