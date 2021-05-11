using System.Threading;
using System.Threading.Tasks;
using Domain.Write.ToDoList.Event;
using MediatR;

namespace Domain.Write.ToDoList.Command
{
    public class AddToDoList : IRequest
    {
        public string Label { get; set; }
        
        public string Description { get; set; }
    }

    public class AddToDoListHandler : IRequestHandler<AddToDoList>
    {
        private readonly IAggregateRepository<ToDoList> _aggregateRepository;
        
        public AddToDoListHandler(IAggregateRepository<ToDoList> aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }
        
        public async Task<Unit> Handle(AddToDoList request, CancellationToken cancellationToken)
        {
            var toDoList = await _aggregateRepository.LoadAsync(null);

            var @event = new ToDoListAddedV1(toDoList.Id, request.Label, request.Description);

            toDoList.When(@event);

            await _aggregateRepository.SaveAsync(toDoList);

            return Unit.Value;
        }
    }
}