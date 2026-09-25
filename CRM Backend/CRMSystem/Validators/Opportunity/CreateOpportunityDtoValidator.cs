using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Opportunity
{
    public class CreateOpportunityDtoValidator : AbstractValidator<CreateOpportunityDto>
    {
        public CreateOpportunityDtoValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Customer ID is required.");

            RuleFor(x => x.PipelineId)
                .NotEmpty()
                .WithMessage("Pipeline ID is required.");

            RuleFor(x => x.StageId)
                .NotEmpty()
                .WithMessage("Stage ID is required.");

            RuleFor(x => x.AssignedUserId)
                .Must(id => id != Guid.Empty)
                .When(x => x.AssignedUserId.HasValue)
                .WithMessage("Assigned user ID must be a valid GUID.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Opportunity name is required.")
                .MaximumLength(255)
                .WithMessage("Opportunity name must not exceed 255 characters.");

            RuleFor(x => x.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Opportunity value cannot be negative.");

            RuleFor(x => x.Description)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("Description must not exceed 5000 characters.");
        }
    }
}