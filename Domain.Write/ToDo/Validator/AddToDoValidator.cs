using Domain.Write.ToDo.Command;
using FluentValidation;

namespace Domain.Write.ToDo.Validator
{
    public class AddToDoValidator : AbstractValidator<AddToDo>
    {
        public AddToDoValidator(IAggregateRepository<ToDoList.ToDoList> toDoListAggregateRepository)
        {
            RuleFor(toDo => toDo.ToDoListId)
                .Custom((toDoListId, context) =>
                {
                    var toDoListAggregate = toDoListAggregateRepository
                        .LoadAsync(toDoListId)
                        .GetAwaiter()
                        .GetResult();

                    var isValidToDoList = toDoListAggregate.GetCommittedEvents().Count > 0;
                        
                    if (!isValidToDoList)
                        context.AddFailure($"The {nameof(ToDoList.ToDoList)} is not valid.");
                });
            
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