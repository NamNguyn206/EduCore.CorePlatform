using FluentValidation;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.Commands.SwitchTenant;

public sealed class SwitchTenantCommandValidator
    : AbstractValidator<SwitchTenantCommand>
{
    public SwitchTenantCommandValidator()
    {
        RuleFor(x => x.RequestData.TenantId)
            .NotEmpty()
            .WithMessage(ErrorCodeConstants.InvalidTenant);
    }
}