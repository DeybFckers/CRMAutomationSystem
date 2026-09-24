using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Customer
{
    public class UpdateCustomerAddressDtoValidator : AbstractValidator<UpdateCustomerAddressDto>
    {
        public UpdateCustomerAddressDtoValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Address type is required.");

            RuleFor(x => x.AddressLine)
                .NotEmpty()
                .WithMessage("Address line is required.");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required.");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Country is required.");

            RuleFor(x => x.Province)
                .Must(value => string.IsNullOrWhiteSpace(value) == false)
                .When(x => x.Province != null)
                .WithMessage("Province cannot be empty or whitespace.");

            RuleFor(x => x.PostalCode)
                .Must(value => string.IsNullOrWhiteSpace(value) == false)
                .When(x => x.PostalCode != null)
                .WithMessage("Postal code cannot be empty or whitespace.");
        }
    }
}
