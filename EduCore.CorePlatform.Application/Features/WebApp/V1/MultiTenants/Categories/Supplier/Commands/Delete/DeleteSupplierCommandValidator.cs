using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Commands.Delete;

public sealed class DeleteSupplierCommandValidator
        : AbstractValidator<DeleteSupplierCommand>
{
    public DeleteSupplierCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData.Ids)
                .NotNull()
                .WithMessage("Ids is required.")
                .NotEmpty()
                .WithMessage("At least one Id must be provided.");
        });
    }
}