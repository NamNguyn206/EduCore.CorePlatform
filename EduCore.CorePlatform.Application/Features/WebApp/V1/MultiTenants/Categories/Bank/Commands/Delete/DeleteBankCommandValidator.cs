using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Commands.Delete;

public sealed class DeleteBankCommandValidator: AbstractValidator<DeleteBankCommand>
{
    public DeleteBankCommandValidator()
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