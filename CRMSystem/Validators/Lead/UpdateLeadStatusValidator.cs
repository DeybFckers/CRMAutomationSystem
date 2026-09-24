using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Lead
{
    public class UpdateLeadStatusDtoValidator : AbstractValidator<UpdateLeadStatusDto>
    {
        public UpdateLeadStatusDtoValidator()
        {
            RuleFor(x => x.StatusId)
                .NotEmpty()
                .WithMessage("Status ID is required.");
        }
    }
}