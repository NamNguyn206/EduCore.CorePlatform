using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Unit.Queries.GetWithPaginatedList;

public class GetUnitWithPaginatedListQueryValidator
    : AbstractValidator<GetUnitWithPaginatedListQuery>
{
    public GetUnitWithPaginatedListQueryValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        When(x => x.RequestData != null, () =>
        {
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
                .When(x => !string.IsNullOrWhiteSpace(x.RequestData.SortBy))
                .WithMessage("SortBy must not exceed 100 characters.");

            RuleFor(x => x.RequestData.SearchTerm)
                .MaximumLength(250)
                .When(x => !string.IsNullOrWhiteSpace(x.RequestData.SearchTerm))
                .WithMessage("SearchTerm must not exceed 250 characters.");

            RuleForEach(x => x.RequestData.Filters)
                .NotNull()
                .When(x => x.RequestData.Filters != null)
                .WithMessage("Filter cannot be null.");
        });
    }
}