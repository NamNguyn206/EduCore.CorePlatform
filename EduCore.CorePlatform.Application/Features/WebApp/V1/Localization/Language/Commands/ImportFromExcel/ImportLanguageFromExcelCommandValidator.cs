using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Commands.ImportFromExcel;

public sealed class ImportLanguageFromExcelCommandValidator : AbstractValidator<ImportLanguageFromExcelCommand>
{
    public ImportLanguageFromExcelCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData!.FileBytes)
                .NotNull()
                .WithMessage("FileBytes must not be null.")
                .Must(x => x.Length > 0)
                .WithMessage("FileBytes must not be empty.");

            RuleFor(x => x.RequestData!.FileName)
                .NotEmpty()
                .WithMessage("FileName must not be empty.")
                .Must(name => name.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Only .xlsx files are allowed.");
        });
    }
}