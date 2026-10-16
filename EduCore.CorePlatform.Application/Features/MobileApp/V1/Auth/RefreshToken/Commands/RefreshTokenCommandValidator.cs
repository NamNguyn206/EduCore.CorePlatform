using EduCore.CorePlatform.Domain.Common.Constants.System;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.RefreshToken.Commands;

public sealed class RefreshTokenCommandValidator
    : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RequestData.RefreshToken)
            .NotEmpty()
            .WithMessage(ErrorCodeConstants.RefreshTokenRequired);

    }
}
