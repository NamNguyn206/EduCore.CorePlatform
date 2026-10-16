using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;


namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.Queries.GetLanguage;

public sealed class GetLanguageByUserIdQueryHandler : IRequestHandler<GetLanguageByUserIdQuery, Result<LanguageDto?>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public GetLanguageByUserIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<LanguageDto?>> Handle(
        GetLanguageByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Identity.User>();

        var spec = new UserByIdForLanguageSpec(request.RequestData.UserId);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<LanguageDto?>.Failure(ErrorCodeConstants.UserNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserNotFound));
        }

        LanguageDto? language = null;
        if (entity.Language != null)
        {
            language = new LanguageDto
            {
                Id = entity.Language.Id,
                Code = entity.Language.Code,
                Name = entity.Language.Name
            };
        }

        return Result<LanguageDto?>.Success(language);
    }
}
