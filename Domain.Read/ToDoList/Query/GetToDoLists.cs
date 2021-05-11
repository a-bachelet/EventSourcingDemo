using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Read.ToDo.Projection;
using Domain.Read.ToDoList.Projection;
using MediatR;

namespace Domain.Read.ToDoList.Query
{
    public class GetToDoListsResponse
    {
        public IQueryable<Entity.ToDoList> Data { get; set; }
    }
    
    public class GetToDoLists : IRequest<GetToDoListsResponse>
    {
    }

    public class GetToDoListsHandler : IRequestHandler<GetToDoLists, GetToDoListsResponse>
    {
        private readonly IToDoListProjectionRepository _repository;
        
        public GetToDoListsHandler(IToDoListProjectionRepository repository)
        {
            _repository = repository;
        }
        
        public Task<GetToDoListsResponse> Handle(GetToDoLists request, CancellationToken cancellationToken)
        {
            var toDoLists = _repository.GetToDoLists();

            return Task.FromResult(new GetToDoListsResponse { Data = toDoLists });
        }
    }
}