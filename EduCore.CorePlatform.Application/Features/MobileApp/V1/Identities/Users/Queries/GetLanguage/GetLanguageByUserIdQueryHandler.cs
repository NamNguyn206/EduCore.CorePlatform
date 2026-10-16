using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.DTOs.Queries;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Specifications.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Queries.GetLanguage;

public sealed class GetLanguageByUserIdQueryHandler : IRequestHandler<GetLanguageByUserIdQuery, Result<UserLanguageDto?>>
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

    public async Task<Result<UserLanguageDto?>> Handle(
        GetLanguageByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Identity.User>();

        var spec = new UserByIdForLanguageSpec(request.RequestData.UserId);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null)
        {
            return Result<UserLanguageDto?>.Failure(ErrorCodeConstants.UserNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserNotFound));
        }

        UserLanguageDto? language = null;
        if (entity.Language != null)
        {
            language = new UserLanguageDto
            {
                Id = entity.Language.Id,
                Code = entity.Language.Code,
                Name = entity.Language.Name
            };
        }

        return Result<UserLanguageDto?>.Success(language);
    }
}
