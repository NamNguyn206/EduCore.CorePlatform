using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Commands.ImportFromExcel;

public sealed class ImportSupplyCategoryFromExcelCommandValidator
        : AbstractValidator<ImportSupplyCategoryFromExcelCommand>
{
    private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB

    public ImportSupplyCategoryFromExcelCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        RuleFor(x => x.RequestData.FileBytes)
            .NotNull()
            .WithMessage("File content is required.")
            .Must(x => x.Length > 0)
            .WithMessage("File must not be empty.")
            .Must(x => x.Length <= MaxFileSizeInBytes)
            .WithMessage("File size must not exceed 5MB.");

        RuleFor(x => x.RequestData.FileName)
            .NotEmpty()
            .WithMessage("FileName is required.")
            .Must(BeValidExcelExtension)
            .WithMessage("Only .xls or .xlsx files are allowed.");
    }

    private static bool BeValidExcelExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        return extension == ".xls" || extension == ".xlsx";
    }
}