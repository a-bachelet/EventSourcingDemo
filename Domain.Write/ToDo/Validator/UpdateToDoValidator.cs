using Domain.Write.ToDo.Command;
using FluentValidation;

namespace Domain.Write.ToDo.Validator
{
    public class UpdateToDoValidator : AbstractValidator<UpdateToDo>
    {
        public UpdateToDoValidator()
        {
            RuleFor(toDo => toDo.Label)
                .NotNull()
                .NotEmpty()
                .Length(3, 255);

            RuleFor(toDo => toDo.Description)
                .NotNull()
                .NotEmpty()
                .Length(3, 255);
        }
    }
}