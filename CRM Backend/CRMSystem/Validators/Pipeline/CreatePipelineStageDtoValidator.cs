using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.PipelineStage
{
    public class CreatePipelineStageDtoValidator : AbstractValidator<CreatePipelineStageDto>
    {
        public CreatePipelineStageDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Pipeline stage name is required.")
                .MaximumLength(255)
                .WithMessage("Pipeline stage name must not exceed 255 characters.");

            RuleFor(x => x.Probability)
                .InclusiveBetween(0, 100)
                .WithMessage("Probability must be between 0 and 100.");

            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Sort order cannot be negative.");

            RuleFor(x => x)
                .Must(x => !(x.IsWon && x.IsLost))
                .WithMessage("A pipeline stage cannot be both won and lost.");
        }
    }
}