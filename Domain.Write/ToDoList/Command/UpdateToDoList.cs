using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Write.ToDoList.Event;
using MediatR;

namespace Domain.Write.ToDoList.Command
{
    public class UpdateToDoList : IRequest
    {
        public Guid ToDoListId { get; set; }
        
        public string Label { get; set; }
        
        public string Description { get; set; }
    }
    
    public class UpdateToDoListHandler : IRequestHandler<UpdateToDoList>
    {
        private readonly IAggregateRepository<ToDoList> _aggregateRepository;
        
        public UpdateToDoListHandler(IAggregateRepository<ToDoList> aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }
        
        public async Task<Unit> Handle(UpdateToDoList request, CancellationToken cancellationToken)
        {
            var toDoList = await _aggregateRepository.LoadAsync(request.ToDoListId);

            var @event = new ToDoListUpdatedV1(toDoList.Id, request.Label, request.Description);

            toDoList.When(@event);

            await _aggregateRepository.SaveAsync(toDoList);

            return Unit.Value;
        }
    }
}