using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Queries.GetById;

public sealed class GetSupplyCategoryByIdQueryValidator
        : AbstractValidator<GetSupplyCategoryByIdQuery>
{
    public GetSupplyCategoryByIdQueryValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        RuleFor(x => x.RequestData.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}