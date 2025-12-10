using FluentValidation;
using Web.Application.Common.Models;

namespace Web.Application.Validation;

public sealed class CreateTodoItemDtoValidator : AbstractValidator<TodoItemDto>
{
    public CreateTodoItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .NotEmpty().WithMessage("Title is required.");
    }

}
