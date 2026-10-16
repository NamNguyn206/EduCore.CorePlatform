using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Commands.ChangeLanguage;

public sealed class ChangeLanguageUserCommandValidator : AbstractValidator<ChangeLanguageUserCommand>
{
    public ChangeLanguageUserCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData!.Id)
                .NotEmpty()
                .WithMessage("Id must not be empty.");

            RuleFor(x => x.RequestData!.LanguageCode)
                .NotEmpty()
                .WithMessage("Name must not be empty.")
                .MaximumLength(200)
                .WithMessage("Name must not exceed 2 characters.");
        });
    }
}