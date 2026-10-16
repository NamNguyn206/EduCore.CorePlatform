using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using EduCore.CorePlatform.Domain.Common.Constants.System;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Commands.InviteExistingUserToTenant;

public sealed class InviteExistingUserToTenantCommandValidator
    : AbstractValidator<InviteExistingUserToTenantCommand>
{
    public InviteExistingUserToTenantCommandValidator()
    {
        RuleFor(x => x.RequestData.UserIds)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage(ErrorCodeConstants.UserRequired);

        RuleFor(x => x.RequestData.RoleIds)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage(ErrorCodeConstants.RoleRequired);

        RuleForEach(x => x.RequestData.UserIds)
            .NotEmpty()
            .WithMessage(ErrorCodeConstants.InvalidUser);

        RuleForEach(x => x.RequestData.RoleIds)
            .NotEmpty()
            .WithMessage(ErrorCodeConstants.InvalidRole);
    }
}