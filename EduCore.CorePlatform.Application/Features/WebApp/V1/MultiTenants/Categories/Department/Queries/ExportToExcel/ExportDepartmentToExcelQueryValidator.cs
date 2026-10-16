using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Queries.ExportToExcel;

public sealed class ExportDepartmentToExcelQueryValidator
    : AbstractValidator<ExportDepartmentToExcelQuery>
{
    public ExportDepartmentToExcelQueryValidator()
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