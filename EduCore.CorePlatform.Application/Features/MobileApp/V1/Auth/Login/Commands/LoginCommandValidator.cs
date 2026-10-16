using EduCore.CorePlatform.Domain.Common.Constants.System;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Login.Commands;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage(ErrorCodeConstants.EmailRequired);

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .WithMessage(ErrorCodeConstants.PasswordRequired);
    }
}


