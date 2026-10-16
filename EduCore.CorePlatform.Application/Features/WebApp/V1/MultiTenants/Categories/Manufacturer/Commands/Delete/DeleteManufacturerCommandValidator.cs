using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Commands.Delete;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Commands.Delete;

public sealed class DeleteManufacturerCommandValidator
    : AbstractValidator<DeleteManufacturerCommand>
{
    public DeleteManufacturerCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData!.Ids)
                .NotNull()
                .WithMessage("Ids must not be null.")
                .NotEmpty()
                .WithMessage("Ids must not be empty.");
        });
    }
}