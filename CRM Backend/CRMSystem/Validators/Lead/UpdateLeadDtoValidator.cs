using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Lead
{
    public class UpdateLeadDtoValidator : AbstractValidator<UpdateLeadDto>
    {
        public UpdateLeadDtoValidator()
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