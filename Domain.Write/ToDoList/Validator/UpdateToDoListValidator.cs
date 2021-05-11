using Domain.Write.ToDoList.Command;
using FluentValidation;

namespace Domain.Write.ToDoList.Validator
{
    public class UpdateToDoListValidator : AbstractValidator<UpdateToDoList>
    {
        public UpdateToDoListValidator()
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