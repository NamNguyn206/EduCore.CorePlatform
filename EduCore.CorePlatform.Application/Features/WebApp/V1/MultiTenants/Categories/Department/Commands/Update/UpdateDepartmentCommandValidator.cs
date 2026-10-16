using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Commands.Update;

public sealed class UpdateDepartmentCommandValidator
    : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData.Id)
                .NotEmpty()
                .WithMessage("Id is required.");

            RuleFor(x => x.RequestData.Name)
                .NotEmpty()
                .WithMessage("Department name is required.")
                .MaximumLength(200)
                .WithMessage("Department name must not exceed 200 characters.")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Department name cannot be whitespace.");

            RuleFor(x => x.RequestData.Address)
                .NotEmpty()
                .WithMessage("Department address is required.")
                .MaximumLength(500)
                .WithMessage("Department address must not exceed 500 characters.")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Department address cannot be whitespace.");
        });
    }
}