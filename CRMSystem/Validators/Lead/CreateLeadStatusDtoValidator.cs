using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Lead
{
    public class CreateLeadStatusDtoValidator : AbstractValidator<CreateLeadStatusDto>
    {
        public CreateLeadStatusDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lead status name is required.")
                .MaximumLength(100)
                .WithMessage("Lead status name must not exceed 100 characters.");

            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Sort order cannot be negative.");
        }
    }
}