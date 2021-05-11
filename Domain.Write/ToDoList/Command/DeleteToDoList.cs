using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Write.ToDoList.Event;
using MediatR;

namespace Domain.Write.ToDoList.Command
{
    public class DeleteToDoList : IRequest
    {
        public Guid ToDoListId { get; set; }
    }

    public class DeleteToDoListHandler : IRequestHandler<DeleteToDoList>
    {
        private readonly IAggregateRepository<ToDoList> _aggregateRepository;
        
        public DeleteToDoListHandler(IAggregateRepository<ToDoList> aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }
        
        public async Task<Unit> Handle(DeleteToDoList request, CancellationToken cancellationToken)
        {
            var toDoList = await _aggregateRepository.LoadAsync(request.ToDoListId);

            var @event = new ToDoListDeletedV1(toDoList.Id);

            toDoList.When(@event);

            await _aggregateRepository.SaveAsync(toDoList);

            return Unit.Value;
        }
    }
}