using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Commands.Update;

public sealed class UpdateSupplierCommandValidator
    : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData is required.");

        RuleFor(x => x.RequestData.Id)
            .NotEmpty()
            .WithMessage("Id is required.");

        RuleFor(x => x.RequestData.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.RequestData.TaxCode)
            .MaximumLength(50)
            .WithMessage("TaxCode must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequestData.TaxCode));

        RuleFor(x => x.RequestData.Address)
            .MaximumLength(500)
            .WithMessage("Address must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequestData.Address));

        RuleFor(x => x.RequestData.Phone)
            .MaximumLength(20)
            .WithMessage("Phone must not exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequestData.Phone));

        RuleFor(x => x.RequestData.Email)
            .EmailAddress()
            .WithMessage("Email is not valid.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequestData.Email));

        RuleFor(x => x.RequestData.BankAccount)
            .MaximumLength(100)
            .WithMessage("BankAccount must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequestData.BankAccount));

        RuleFor(x => x.RequestData.BankBranch)
            .MaximumLength(200)
            .WithMessage("BankBranch must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequestData.BankBranch));

        RuleFor(x => x.RequestData.Note)
            .MaximumLength(1000)
            .WithMessage("Note must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RequestData.Note));

        RuleFor(x => x.RequestData.BankId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("BankId is invalid.");

        RuleFor(x => x.RequestData.SupplyCategoryId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("SupplyCategoryId is invalid.");
    }
}