using Domain.Write.ToDoList.Command;
using FluentValidation;

namespace Domain.Write.ToDoList.Validator
{
    public class DeleteToDoListValidator : AbstractValidator<DeleteToDoList>
    {
        public DeleteToDoListValidator()
        {
        }
    }
}