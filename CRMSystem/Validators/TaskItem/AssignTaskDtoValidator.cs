using CRMSystem.Models.DTOs;
using FluentValidation;

namespace CRMSystem.Validators.TaskItem
{
    public class AssignTaskDtoValidator : AbstractValidator<AssignTaskDto>
    {
        public AssignTaskDtoValidator()
        {
            RuleFor(x => x.AssignedUserId)
                .NotEmpty()
                .WithMessage("Assigned user ID is required.");
        }
    }
}