using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Lead
{
    public class CreateLeadSourceDtoValidator : AbstractValidator<CreateLeadSourceDto>
    {
        public CreateLeadSourceDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lead source name is required.")
                .MaximumLength(255)
                .WithMessage("Lead source name must not exceed 255 characters.");
        }
    }
}