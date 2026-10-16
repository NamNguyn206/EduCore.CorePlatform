using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Commands.Update;

public sealed class UpdateLeadCommandValidator
    : AbstractValidator<UpdateLeadCommand>
{
    public UpdateLeadCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData!.Id)
                .NotEmpty()
                .WithMessage("Id must not be empty.");

            RuleFor(x => x.RequestData!.FullName)
                .NotEmpty()
                .WithMessage("FullName must not be empty.")
                .MaximumLength(150)
                .WithMessage("FullName must not exceed 150 characters.");

            RuleFor(x => x.RequestData!.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.RequestData!.Email))
                .WithMessage("Invalid email format.");

            RuleFor(x => x.RequestData!.PhoneNumber)
                .MaximumLength(20)
                .WithMessage("PhoneNumber must not exceed 20 characters.");

            RuleFor(x => x.RequestData!.PredictionPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("PredictionPercentage must be between 0 and 100.");

            RuleFor(x => x.RequestData!.Temperature)
                .IsInEnum()
                .WithMessage("Invalid Temperature status.");
        });
    }
}