using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Commands.Delete;

public sealed class DeleteDepartmentCommandValidator
    : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentCommandValidator()
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