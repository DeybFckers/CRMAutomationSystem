using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Opportunity
{
    public class UpdateOpportunityStageDtoValidator
        : AbstractValidator<UpdateOpportunityStageDto>
    {
        public UpdateOpportunityStageDtoValidator()
        {
            RuleFor(x => x.StageId)
                .NotEmpty()
                .WithMessage("Stage ID is required.");
        }
    }
}