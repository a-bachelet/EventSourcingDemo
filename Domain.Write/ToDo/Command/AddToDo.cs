using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Write.ToDo.Event;
using MediatR;

namespace Domain.Write.ToDo.Command
{
    public class AddToDo : IRequest
    {
        public Guid ToDoListId { get; set; }
        
        public string Label { get; set; }
        
        public string Description { get; set; }
    }

    public class AddToDoHandler : IRequestHandler<AddToDo>
    {
        private readonly IAggregateRepository<ToDo> _aggregateRepository;
        
        public AddToDoHandler(IAggregateRepository<ToDo> aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }
        
        public async Task<Unit> Handle(AddToDo request, CancellationToken cancellationToken)
        {
            var toDo = await _aggregateRepository.LoadAsync(null);

            var @event = new ToDoAddedV1(toDo.Id, request.ToDoListId, request.Label, request.Description);

            toDo.When(@event);

            await _aggregateRepository.SaveAsync(toDo);

            return Unit.Value;
        }
    }
}