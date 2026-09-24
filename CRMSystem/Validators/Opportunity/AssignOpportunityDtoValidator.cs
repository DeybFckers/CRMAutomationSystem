using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Opportunity
{
    public class AssignOpportunityDtoValidator
        : AbstractValidator<AssignOpportunityDto>
    {
        public AssignOpportunityDtoValidator()
        {
            RuleFor(x => x.AssignedUserId)
                .Must(id => id != Guid.Empty)
                .When(x => x.AssignedUserId.HasValue)
                .WithMessage("Assigned user ID must be a valid GUID.");
        }
    }
}