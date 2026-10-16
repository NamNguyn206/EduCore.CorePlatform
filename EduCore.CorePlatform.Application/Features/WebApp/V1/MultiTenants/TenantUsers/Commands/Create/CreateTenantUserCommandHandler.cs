using MediatR;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Commands.Create;

public sealed class CreateTenantUserCommandHandler: IRequestHandler<CreateTenantUserCommand, Result<TenantUserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public CreateTenantUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ICurrentTenant currentTenant,
        ICurrentUser currentUser,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<TenantUserDto>> Handle(
        CreateTenantUserCommand request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentTenant.TenantId!.Value;

        if (tenantId == Guid.Empty)
        {
            return Result<TenantUserDto>.Failure(ErrorCodeConstants.InvalidTenant, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidTenant));
        }

        var repoUser = _unitOfWork.Repository<User>();
        var repoRole = _unitOfWork.Repository<Role>();

        var email = request.RequestData.Email.Trim().ToLower();

        // 1️ Kiểm tra email đã tồn tại
        var existingUser = await repoUser.FirstOrDefaultAsync(new UserByEmailForCreateTenantUserSpec(email), cancellationToken);

        if (existingUser != null)
        {
            return Result<TenantUserDto>.Failure(ErrorCodeConstants.UserAlreadyExists, await _systemMessageProvider.GetAsync(ErrorCodeConstants.UserAlreadyExists));
        }

        // 2️ Load Roles
        var roles = await repoRole.ListAsync(new RolesByIdsForCreateTenantUserSpec(request.RequestData.RoleIds), cancellationToken);

        if (roles.Count != request.RequestData.RoleIds.Count)
        {
            return Result<TenantUserDto>.Failure(ErrorCodeConstants.InvalidRole, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidRole));
        }

        // 3️ Validate Role thuộc System hoặc Tenant hiện tại
        foreach (var role in roles)
        {
            if (role.Scope == RoleScope.System)
                continue;

            if (role.Scope == RoleScope.Tenant &&
                role.TenantId == tenantId)
                continue;

            return Result<TenantUserDto>.Failure(ErrorCodeConstants.InvalidRole, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidRole));
        }

        // 4️ Hash password
        var passwordHash = _passwordHasher.Hash(request.RequestData.Password);

        // 5️ Tạo User (KHÔNG phải SuperAdmin)
        var user = new User(
            email,
            passwordHash,
            request.RequestData.FirstName,
            request.RequestData.LastName
        );

        // 6️ Tạo UserTenant
        user.AddToTenant(tenantId, request.RequestData.IsTenantAdmin, request.RequestData.RoleIds);

        // 7 Add vào DB
        await repoUser.AddAsync(user, cancellationToken);

        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return Result<TenantUserDto>.Success(new TenantUserDto() { 
            UserId = user.Id
        });
    }
}