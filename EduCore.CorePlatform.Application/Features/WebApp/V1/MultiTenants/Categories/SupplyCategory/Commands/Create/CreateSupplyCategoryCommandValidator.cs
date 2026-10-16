using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Commands.Create;

public sealed class CreateSupplyCategoryCommandValidator
    : AbstractValidator<CreateSupplyCategoryCommand>
{
    public CreateSupplyCategoryCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        RuleFor(x => x.RequestData.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters.");
    }
}