using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Note
{
    public class CreateNoteDtoValidator : AbstractValidator<CreateNoteDto>
    {
        public CreateNoteDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Note content is required.")
                .MaximumLength(5000)
                .WithMessage("Note content must not exceed 5000 characters.");

            RuleFor(x => x)
                .Must(x =>
                    x.CustomerId.HasValue ||
                    x.LeadId.HasValue ||
                    x.OpportunityId.HasValue)
                .WithMessage(
                    "A note must be associated with a customer, lead, or opportunity.");
        }
    }
}