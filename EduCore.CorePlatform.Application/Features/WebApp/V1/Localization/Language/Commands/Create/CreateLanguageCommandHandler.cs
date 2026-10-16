using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Commands;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Commands.Create;

public sealed class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, Result<LanguageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public CreateLanguageCommandHandler(IUnitOfWork unitOfWork,
                                        ICurrentUser currentUser,
                                    ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _systemMessageProvider = systemMessageProvider;
        _currentUser = currentUser;
    }
    public async Task<Result<LanguageDto>> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Localization.Language>();

        var spec = new LanguageByCodeForCreateSpec(request.RequestData.Code);

        var existing = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (existing != null)
            return Result<LanguageDto>.Failure(ErrorCodeConstants.LanguageCodeExists, await _systemMessageProvider.GetAsync(ErrorCodeConstants.LanguageCodeExists));

        var entity = new Domain.Entities.Localization.Language(
            request.RequestData.Code, 
            request.RequestData.Name,
            false,
            _currentUser.UserId
        );

        await repo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result<LanguageDto>.Success(new LanguageDto()
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}