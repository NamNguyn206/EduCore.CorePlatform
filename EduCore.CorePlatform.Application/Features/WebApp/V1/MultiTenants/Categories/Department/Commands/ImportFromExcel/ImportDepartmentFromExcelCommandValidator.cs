using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using System.IO;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.DTOs.Commands;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Commands.ImportFromExcel;

public sealed class ImportDepartmentFromExcelCommandValidator
    : AbstractValidator<ImportDepartmentFromExcelCommand>
{
    private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB

    public ImportDepartmentFromExcelCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        When(x => x.RequestData != null, () =>
        {
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
        });
    }

    private static bool BeValidExcelExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        return extension == ".xls" || extension == ".xlsx";
    }
}