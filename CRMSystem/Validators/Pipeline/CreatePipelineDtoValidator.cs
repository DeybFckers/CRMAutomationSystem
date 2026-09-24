using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.Pipeline
{
    public class CreatePipelineDtoValidator : AbstractValidator<CreatePipelineDto>
    {
        public CreatePipelineDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Pipeline name is required.")
                .MaximumLength(255)
                .WithMessage("Pipeline name must not exceed 255 characters.");
        }
    }
}