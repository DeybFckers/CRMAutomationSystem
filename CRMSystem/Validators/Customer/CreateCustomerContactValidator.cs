using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Customer
{
    public class CreateCustomerContactDtoValidator : AbstractValidator<CreateCustomerContactDto>
    {
        public CreateCustomerContactDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .Must(value => string.IsNullOrWhiteSpace(value) == false)
                .When(x => x.FirstName != null)
                .WithMessage("First name cannot be empty or whitespace.");

            RuleFor(x => x.LastName)
                .Must(value => string.IsNullOrWhiteSpace(value) == false)
                .When(x => x.LastName != null)
                .WithMessage("Last name cannot be empty or whitespace.");

            RuleFor(x => x.Position)
                .Must(value => string.IsNullOrWhiteSpace(value) == false)
                .When(x => x.Position != null)
                .WithMessage("Position cannot be empty or whitespace.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email must be a valid email address.");

            RuleFor(x => x.Phone)
                .Must(value => string.IsNullOrWhiteSpace(value) == false)
                .When(x => x.Phone != null)
                .WithMessage("Phone cannot be empty or whitespace.");
        }
    }
}
