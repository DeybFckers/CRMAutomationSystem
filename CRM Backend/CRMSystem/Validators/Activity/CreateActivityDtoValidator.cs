using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Activity
{
    public class CreateActivityDtoValidator : AbstractValidator<CreateActivityDto>
    {
        public CreateActivityDtoValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Activity type is required.")
                .MaximumLength(50)
                .WithMessage("Activity type must not exceed 50 characters.");

            RuleFor(x => x.Subject)
                .NotEmpty()
                .WithMessage("Activity subject is required.")
                .MaximumLength(255)
                .WithMessage("Activity subject must not exceed 255 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("Activity description must not exceed 5000 characters.");

            RuleFor(x => x.ActivityDate)
                .NotEmpty()
                .WithMessage("Activity date is required.");

            RuleFor(x => x)
                .Must(x =>
                    Convert.ToInt32(x.CustomerId.HasValue) +
                    Convert.ToInt32(x.LeadId.HasValue) +
                    Convert.ToInt32(x.OpportunityId.HasValue) == 1)
                .WithMessage(
                    "An activity must be associated with exactly one customer, lead, or opportunity.");
        }
    }
}