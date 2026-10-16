using FluentValidation;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.Create;

namespace EduCore.CorePlatform.Application.Features.Tenants.Commands.Create;

public class CreateTenantCommandValidator
    : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        RuleFor(x => x.RequestData.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.RequestData.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.RequestData.Description)
            .MaximumLength(1000);
    }
}