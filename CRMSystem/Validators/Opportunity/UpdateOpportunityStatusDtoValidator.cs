using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Opportunity
{
    public class UpdateOpportunityStatusDtoValidator
        : AbstractValidator<UpdateOpportunityStatusDto>
    {
        public UpdateOpportunityStatusDtoValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Opportunity status is required.")
                .Must(status =>
                    status.Equals("OPEN", StringComparison.OrdinalIgnoreCase) ||
                    status.Equals("WON", StringComparison.OrdinalIgnoreCase) ||
                    status.Equals("LOST", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Opportunity status must be OPEN, WON, or LOST.");
        }
    }
}