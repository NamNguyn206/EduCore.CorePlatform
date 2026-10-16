using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Common.Services.Security;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Login.DTOs;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Login.Specifications;
using EduCore.CorePlatform.Application.Features.MobileApp.V1.Localization.Language.DTOs;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using EduCore.CorePlatform.Domain.Entities.Identity;
using MediatR;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Login.Commands;

public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ISystemMessageProvider _systemMessageProvider;
    private readonly IUserAuthorizationService _authorizationService;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        ISystemMessageProvider systemMessageProvider,
        IUserAuthorizationService authorizationService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _systemMessageProvider = systemMessageProvider;
        _authorizationService = authorizationService;
    }

    public async Task<Result<LoginResponseDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var repoUser = _unitOfWork.Repository<User>();
        var repoRefreshToken = _unitOfWork.Repository<Domain.Entities.Identity.RefreshToken>();
        
        var email = request.Request.Email.Trim().ToLower();

        // =====================================================
        // FIND USER
        // =====================================================
        var spec = new UserByEmailWithDetailsSpec(email);
        var user = await repoUser.FirstOrDefaultAsync(spec, cancellationToken);

        if (user == null)
        {
            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.InvalidCredentials, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidCredentials));
        }

        // =====================================================
        // CHECK EMAIL
        // =====================================================

        if (!user.EmailConfirmed)
        {
            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.EmailNotConfirmed, await _systemMessageProvider.GetAsync(ErrorCodeConstants.EmailNotConfirmed));
        }

        // =====================================================
        // CHECK LOCK
        // =====================================================
        if (user.IsLocked())
        {
            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.UserLocked, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserLocked));
        }

        // =====================================================
        // USER STATUS CHECK
        // =====================================================
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

        // Lưu lại tenant hiện tại nếu chưa có
        user.SetLastTenant(currentTenantId);

        // =====================================================
        // PASSWORD CHECK
        // =====================================================
        var passwordValid = _passwordHasher.Verify(request.Request.Password, user.PasswordHash);

        if (!passwordValid)
        {
            user.RecordFailedLogin();
            await _unitOfWork.SaveEntitiesAsync(cancellationToken);

            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.InvalidCredentials, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidCredentials));
        }

        user.RecordSuccessfulLogin();

        if (!string.IsNullOrWhiteSpace(request.Request.DeviceId))
        {
            user.SetDeviceId(request.Request.DeviceId);
        }

        // =====================================================
        // MOBILE APP AUTHORIZATION
        // =====================================================

        if (!user.CanAccessMobileApp())
        {
            return Result<LoginResponseDto>.Failure(ErrorCodeConstants.MobileAppAccessDenied, await _systemMessageProvider.GetAsync(ErrorCodeConstants.MobileAppAccessDenied));
        }

        // =====================================================
        // GENERATE TOKEN
        // =====================================================

        var accessToken = _tokenService.GenerateAccessToken(user);

        var refreshToken = _tokenService.GenerateRefreshToken(user.Id, ClientTypeConstants.MobileApp, request.Request.DeviceId);

        await repoRefreshToken.AddAsync(refreshToken, cancellationToken);

        // =====================================================
        // SAVE
        // =====================================================
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        // =====================================================
        // LOAD TENANTS
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

        // =====================================================
        // 📦 RESPONSE
        // =====================================================
        return Result<LoginResponseDto>.Success(
            new LoginResponseDto
            {
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Token,
                ExpiresAt = accessToken.ExpiresAt,
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
                    Language = user.Language != null ? new LanguageDto() { 
                        Id = user.Language.Id,
                        Code = user.Language.Code,
                        Name = user.Language.Name,
                    } : null
                },
                Tenants = tenantDtos
            });
    }

}
