using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using EduCore.CorePlatform.Domain.Common.Constants.System;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Commands.Create;

public sealed class CreateTenantUserCommandValidator
    : AbstractValidator<CreateTenantUserCommand>
{
    public CreateTenantUserCommandValidator()
    {
        RuleFor(x => x.RequestData.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage(ErrorCodeConstants.EmailRequired);

        RuleFor(x => x.RequestData.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage(ErrorCodeConstants.PasswordRequired);

        RuleFor(x => x.RequestData.RoleIds)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage(ErrorCodeConstants.RoleRequired);
    }
}