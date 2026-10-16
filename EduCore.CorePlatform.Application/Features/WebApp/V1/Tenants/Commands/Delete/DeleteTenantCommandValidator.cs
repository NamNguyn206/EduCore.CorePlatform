using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.Tenants.Commands.Delete;

public sealed class DeleteTenantCommandValidator
    : AbstractValidator<DeleteTenantCommand>
{
    public DeleteTenantCommandValidator()
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