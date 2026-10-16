using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Queries.GetWithPaginatedList;

public sealed class GetSupplierWithPaginatedListQueryValidator
    : AbstractValidator<GetSupplierWithPaginatedListQuery>
{
    public GetSupplierWithPaginatedListQueryValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        RuleFor(x => x.RequestData.PageIndex)
            .GreaterThan(0)
            .WithMessage("PageIndex must be greater than 0.");

        RuleFor(x => x.RequestData.PageSize)
            .GreaterThan(0)
            .WithMessage("PageSize must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("PageSize must not exceed 100.");

        RuleFor(x => x.RequestData.SortBy)
            .MaximumLength(100)
            .WithMessage("SortBy must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequestData.SortBy));

        RuleFor(x => x.RequestData.SearchTerm)
            .MaximumLength(250)
            .WithMessage("SearchTerm must not exceed 250 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequestData.SearchTerm));

        RuleForEach(x => x.RequestData.Filters)
            .NotNull()
            .When(x => x.RequestData.Filters != null)
            .WithMessage("Filter cannot be null.");
    }
}