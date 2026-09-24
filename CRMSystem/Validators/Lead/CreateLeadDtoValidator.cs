using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Lead
{
    public class CreateLeadDtoValidator : AbstractValidator<CreateLeadDto>
    {
        public CreateLeadDtoValidator()
        {
            RuleFor(x => x.AssignedUserId)
                .Must(id => id != Guid.Empty)
                .When(x => x.AssignedUserId.HasValue)
                .WithMessage("Assigned user ID must be a valid GUID.");

            RuleFor(x => x.SourceId)
                .NotEmpty()
                .WithMessage("Source ID is required.");

            RuleFor(x => x.StatusId)
                .NotEmpty()
                .WithMessage("Status ID is required.");

            RuleFor(x => x.FirstName)
                .MaximumLength(100)
                .WithMessage("First name must not exceed 100 characters.");

            RuleFor(x => x.LastName)
                .MaximumLength(100)
                .WithMessage("Last name must not exceed 100 characters.");

            RuleFor(x => x.CompanyName)
                .MaximumLength(255)
                .WithMessage("Company name must not exceed 255 characters.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email must be a valid email address.");

            RuleFor(x => x.Email)
                .MaximumLength(255)
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email must not exceed 255 characters.");

            RuleFor(x => x.Phone)
                .MaximumLength(30)
                .When(x => !string.IsNullOrWhiteSpace(x.Phone))
                .WithMessage("Phone number must not exceed 30 characters.");

            RuleFor(x => x.EstimatedValue)
                .GreaterThanOrEqualTo(0)
                .When(x => x.EstimatedValue.HasValue)
                .WithMessage("Estimated value cannot be negative.");

            RuleFor(x => x.Notes)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes must not exceed 5000 characters.");
        }
    }
}