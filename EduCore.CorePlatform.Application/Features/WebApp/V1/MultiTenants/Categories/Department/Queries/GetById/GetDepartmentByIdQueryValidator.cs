using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Queries.GetById;

public sealed class GetDepartmentByIdQueryValidator
        : AbstractValidator<GetDepartmentByIdQuery>
{
    public GetDepartmentByIdQueryValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        RuleFor(x => x.RequestData.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}