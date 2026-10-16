using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Queries.ExportToExcel;

public sealed class ExportSupplyCategoryToExcelQueryValidator
        : AbstractValidator<ExportSupplyCategoryToExcelQuery>
{
    public ExportSupplyCategoryToExcelQueryValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        RuleFor(x => x.RequestData.SearchTerm)
            .MaximumLength(250)
            .WithMessage("SearchTerm must not exceed 250 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequestData.SearchTerm));
    }
}