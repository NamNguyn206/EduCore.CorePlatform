using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Queries.ExportToExcel;

public sealed class ExportManufacturerToExcelQueryValidator
    : AbstractValidator<ExportManufacturerToExcelQuery>
{
    public ExportManufacturerToExcelQueryValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData!.SearchTerm)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.RequestData!.SearchTerm))
                .WithMessage("SearchTerm must not exceed 200 characters.");
        });
    }
}