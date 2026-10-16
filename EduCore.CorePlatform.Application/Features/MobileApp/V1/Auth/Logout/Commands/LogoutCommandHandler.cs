using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Logout.Specifications;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Logout.Commands;

public sealed class LogoutCommandHandler: IRequestHandler<LogoutCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public LogoutCommandHandler(
        IUnitOfWork unitOfWork,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RequestData.RefreshToken))
            return Result.Failure(ErrorCodeConstants.InvalidRefreshToken, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidRefreshToken));

        var repo = _unitOfWork.Repository<Domain.Entities.Identity.RefreshToken>();
        var spec = new LogoutRefreshTokenSpec(request.RequestData.RefreshToken);

        // 1️ Tìm refresh token
        var refreshToken = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        // ⭐ Logout idempotent → không báo lỗi nếu token không tồn tại
        if (refreshToken == null)
            return Result.Success();

        // 2️ Nếu đã revoke rồi → coi như logout xong
        if (refreshToken.IsRevoked)
        {
            return Result.Success(string.Empty);
        }

        // 3️ Revoke refresh token
        if (!refreshToken.IsRevoked)
        {
            refreshToken.Revoke();
            await _unitOfWork.SaveEntitiesAsync(cancellationToken);
        }
        
        return Result.Success(string.Empty);
    }
}
