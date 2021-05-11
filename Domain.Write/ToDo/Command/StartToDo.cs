using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Write.ToDo.Event;
using MediatR;

namespace Domain.Write.ToDo.Command
{
    public class StartToDo : IRequest
    {
        public Guid ToDoId { get; set; }
    }

    public class StartToDoHandler : IRequestHandler<StartToDo>
    {
        private readonly IAggregateRepository<ToDo> _aggregateRepository;
        
        public StartToDoHandler(IAggregateRepository<ToDo> aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }
        
        public async Task<Unit> Handle(StartToDo request, CancellationToken cancellationToken)
        {
            var toDo = await _aggregateRepository.LoadAsync(request.ToDoId);

            var @event = new ToDoStartedV1(toDo.Id, toDo.State.ToDoListId);

            toDo.When(@event);

            await _aggregateRepository.SaveAsync(toDo);

            return Unit.Value;
        }
    }
}