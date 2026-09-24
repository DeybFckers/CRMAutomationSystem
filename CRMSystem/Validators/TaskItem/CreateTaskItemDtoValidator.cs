using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.TaskItem
{
    public class CreateTaskItemDtoValidator : AbstractValidator<CreateTaskItemDto>
    {
        public CreateTaskItemDtoValidator()
        {
            RuleFor(x => x.AssignedUserId)
                .NotEmpty()
                .WithMessage("Assigned user ID is required.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Task title is required.")
                .MaximumLength(255)
                .WithMessage("Task title must not exceed 255 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("Task description must not exceed 5000 characters.");

            RuleFor(x => x.DueDate)
                .NotEmpty()
                .WithMessage("Due date is required.");

            RuleFor(x => x.Priority)
                .NotEmpty()
                .WithMessage("Task priority is required.")
                .Must(priority =>
                    priority.Equals("LOW", StringComparison.OrdinalIgnoreCase) ||
                    priority.Equals("MEDIUM", StringComparison.OrdinalIgnoreCase) ||
                    priority.Equals("HIGH", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Task priority must be LOW, MEDIUM, or HIGH.");

            RuleFor(x => x)
                .Must(x =>
                    Convert.ToInt32(x.CustomerId.HasValue) +
                    Convert.ToInt32(x.LeadId.HasValue) +
                    Convert.ToInt32(x.OpportunityId.HasValue) <= 1)
                .WithMessage(
                    "A task can be associated with at most one customer, lead, or opportunity.");
        }
    }
}