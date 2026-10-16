using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.Tenants.Queries.GetDefaultLanguage;

public sealed class GetDefaultLanguageByTenantIdQueryValidator : AbstractValidator<GetDefaultLanguageByTenantIdQuery>
{
    public GetDefaultLanguageByTenantIdQueryValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData!.TenantId)
                .NotEmpty()
                .WithMessage("TenantId must not be empty.");
        });
    }
}