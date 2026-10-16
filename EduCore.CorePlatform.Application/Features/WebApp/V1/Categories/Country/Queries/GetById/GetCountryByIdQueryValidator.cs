using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Queries.GetById;

public sealed class GetCountryByIdQueryValidator
    : AbstractValidator<GetCountryByIdQuery>
{
    public GetCountryByIdQueryValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData!.Id)
                .NotEmpty()
                .WithMessage("Id must not be empty.");
        });
    }
}