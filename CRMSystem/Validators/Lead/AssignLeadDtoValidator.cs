using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Lead
{
    public class AssignLeadDtoValidator : AbstractValidator<AssignLeadDto>
    {
        public AssignLeadDtoValidator()
        {
            RuleFor(x => x.AssignedUserId)
                .NotEmpty()
                .WithMessage("Assigned user ID is required.");
        }
    }
}