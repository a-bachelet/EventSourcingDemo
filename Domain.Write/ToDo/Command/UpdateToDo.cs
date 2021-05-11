using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Write.ToDo.Event;
using MediatR;

namespace Domain.Write.ToDo.Command
{
    public class UpdateToDo : IRequest
    {
        public Guid ToDoId { get; set; }
        
        public string Label { get; set; }
        
        public string Description { get; set; }
    }
    
    public class UpdateToDoHandler : IRequestHandler<UpdateToDo>
    {
        private readonly IAggregateRepository<ToDo> _aggregateRepository;
        
        public UpdateToDoHandler(IAggregateRepository<ToDo> aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }
        
        public async Task<Unit> Handle(UpdateToDo request, CancellationToken cancellationToken)
        {
            var toDo = await _aggregateRepository.LoadAsync(request.ToDoId);

            var @event = new ToDoUpdatedV1(toDo.Id, toDo.State.ToDoListId, request.Label, request.Description);

            toDo.When(@event);

            await _aggregateRepository.SaveAsync(toDo);

            return Unit.Value;
        }
    }
}