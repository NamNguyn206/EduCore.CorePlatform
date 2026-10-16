using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Commands.Update;

public sealed class UpdateSupplyCategoryCommandValidator
        : AbstractValidator<UpdateSupplyCategoryCommand>
{
    public UpdateSupplyCategoryCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        RuleFor(x => x.RequestData.Id)
            .NotEmpty()
            .WithMessage("Id is required.");

        RuleFor(x => x.RequestData.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters.");
    }
}