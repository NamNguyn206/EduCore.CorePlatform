using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Common.Services.Security;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.RefreshToken.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.RefreshToken.Specifications;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.RefreshToken.Commands;

public sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, Result<LoginResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ISystemMessageProvider _systemMessageProvider;
    private readonly IUserAuthorizationService _authorizationService;
    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        ISystemMessageProvider systemMessageProvider,
        IUserAuthorizationService authorizationService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _systemMessageProvider = systemMessageProvider;
        _authorizationService = authorizationService;
    }

    public async Task<Result<LoginResponseDto>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var repoRefreshToken = _unitOfWork.Repository<Domain.Entities.Identity.RefreshToken>();

        var spec = new RefreshTokenWithUserDetailsSpec(request.RequestData.RefreshToken);

        // 1️ Tìm refresh token
        var refreshToken = await repoRefreshToken.FirstOrDefaultAsync(spec, cancellationToken);
        
        if (refreshToken == null)
        {
            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.InvalidRefreshToken, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidRefreshToken));
        }

        // =========================
        // 2. Validate refresh token, Kiểm tra trạng thái
        // =========================
        if (refreshToken.IsRevoked)
            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.RefreshTokenRevoked, await _systemMessageProvider.GetAsync(ErrorCodeConstants.RefreshTokenRevoked));

        if (refreshToken.ExpiresAt <= DateTime.UtcNow)
            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.RefreshTokenExpired, await _systemMessageProvider.GetAsync(ErrorCodeConstants.RefreshTokenExpired));

        // 3️ Lấy user
        var user = refreshToken.User;
        if (user == null || !user.IsValid())
        {
            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.UserNotFound, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserNotFound));
        }

        if (user.IsLocked())
        {
            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.UserLocked, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserLocked));
        }

        switch (user.Status)
        {
            case UserStatus.PendingActivation:
                return Result<LoginResponseDto>.Failure(ErrorCodeConstants.UserPendingActivation, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserPendingActivation));

            case UserStatus.Inactive:
                return Result<LoginResponseDto>.Failure(ErrorCodeConstants.UserInactive, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserInactive));

            case UserStatus.Suspended:
                return Result<LoginResponseDto>.Failure(ErrorCodeConstants.UserSuspended, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserSuspended));

            case UserStatus.Deleted:
                return Result<LoginResponseDto>.Failure(ErrorCodeConstants.UserDeleted, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserDeleted));
        }

        // =====================================================
        // CHECK WEB APP PERMISSION
        // =====================================================

        if (!user.CanAccessWebApp())
        {
            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.WebAppAccessDenied, await _systemMessageProvider.GetAsync(ErrorCodeConstants.WebAppAccessDenied));
        }

        // =====================================================
        // CURRENT TENANT
        // =====================================================

        var currentTenantId = user.LastTenantId ??
                                user.UserTenants
                                    .Where(x => x.LeftAt == null)
                                    .Select(x => x.TenantId)
                                    .FirstOrDefault();

        if (currentTenantId == Guid.Empty)
        {
            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.NoTenantAssigned, await _systemMessageProvider.GetAsync(ErrorCodeConstants.NoTenantAssigned));
        }

        // 4️ Revoke refresh token cũ
        refreshToken.Revoke();
        repoRefreshToken.Update(refreshToken);

        // 5️ Sinh token mới
        var newAccessToken = _tokenService.GenerateAccessToken(user);

        var newRefreshToken = _tokenService.GenerateRefreshToken(user.Id, ClientTypeConstants.WebApp, string.Empty);

        await repoRefreshToken.AddAsync(newRefreshToken, cancellationToken);

        // 6️ Save transaction
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        // =====================================================
        // USER TENANTS
        // =====================================================

        var tenantDtos = user.UserTenants
            .Where(x => x.LeftAt == null)
            .Select(userTenant =>
            {
                var roles = _authorizationService.GetRoles(user, userTenant.TenantId);

                var roleDtos = roles
                    .Select(role => new RoleDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Description = role.Description,
                        Permissions = role.RolePermissions
                            .Select(rp => rp.Permission.Code)
                            .Distinct()
                            .ToList()
                    })
                    .ToList();

                return new UserTenantDto
                {
                    Tenant = new TenantDto
                    {
                        Id = userTenant.TenantId,
                        Name = userTenant.Tenant.Name
                    },

                    IsCurrentTenant = userTenant.TenantId == currentTenantId,
                    IsTenantAdmin = userTenant.IsTenantAdmin,
                    Roles = roleDtos
                };
            })
            .ToList();

        // 7️ Trả response
        return Result<LoginResponseDto>.Success(
            new LoginResponseDto
            {
                AccessToken = newAccessToken.Token,
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = newAccessToken.ExpiresAt,
                User = new UserProfileDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.GetFullName(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserType = user.UserType,
                    VerificationStatus = user.VerificationStatus,
                    Language = user.Language != null ? new LanguageDto()
                    {
                        Id = user.Language.Id,
                        Code = user.Language.Code,
                        Name = user.Language.Name,
                    } : null
                },
                Tenants = tenantDtos
            });
    }
}
