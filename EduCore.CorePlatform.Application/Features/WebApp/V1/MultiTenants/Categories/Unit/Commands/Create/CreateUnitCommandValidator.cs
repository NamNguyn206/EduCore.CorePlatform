using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Commands.Create;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Commands.Create;

public sealed class CreateUnitCommandValidator
    : AbstractValidator<CreateUnitCommand>
{
    public CreateUnitCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData!.Name)
                .NotEmpty()
                .WithMessage("Name must not be empty.")
                .MaximumLength(200)
                .WithMessage("Name must not exceed 200 characters.");
        });
    }
}