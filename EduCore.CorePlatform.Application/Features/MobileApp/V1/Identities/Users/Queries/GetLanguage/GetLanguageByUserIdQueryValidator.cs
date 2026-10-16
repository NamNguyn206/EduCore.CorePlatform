using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Identities.Users.Queries.GetLanguage;

public sealed class GetLanguageByUserIdQueryValidator : AbstractValidator<GetLanguageByUserIdQuery>
{
    public GetLanguageByUserIdQueryValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData!.UserId)
                .NotEmpty()
                .WithMessage("UserId must not be empty.");
        });
    }
}