using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Write.ToDo.Event;
using MediatR;

namespace Domain.Write.ToDo.Command
{
    public class DeleteToDo : IRequest
    {
        public Guid ToDoId { get; set; }
    }
    
    public class DeleteToDoHandler : IRequestHandler<DeleteToDo>
    {
        private readonly IAggregateRepository<ToDo> _aggregateRepository;
        
        public DeleteToDoHandler(IAggregateRepository<ToDo> aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }
        
        public async Task<Unit> Handle(DeleteToDo request, CancellationToken cancellationToken)
        {
            var toDo = await _aggregateRepository.LoadAsync(request.ToDoId);

            var @event = new ToDoDeletedV1(toDo.Id, toDo.State.ToDoListId);

            toDo.When(@event);

            await _aggregateRepository.SaveAsync(toDo);

            return Unit.Value;
        }
    }
}