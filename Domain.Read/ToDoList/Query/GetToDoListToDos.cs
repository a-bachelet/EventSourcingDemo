using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Read.ToDoList.Projection;
using MediatR;

namespace Domain.Read.ToDoList.Query
{
    public class GetToDoListToDosResponse
    {
        public IQueryable<ToDo.Entity.ToDo> Data { get; set; }
    }
    
    public class GetToDoListToDos : IRequest<GetToDoListToDosResponse>
    {
        public Guid ToDoListId { get; set; }
    }
    
    public class GetToDoListToDosHandler : IRequestHandler<GetToDoListToDos, GetToDoListToDosResponse>
    {
        private readonly IToDoListProjectionRepository _repository;

        public GetToDoListToDosHandler(IToDoListProjectionRepository repository)
        {
            _repository = repository;
        }
        
        public Task<GetToDoListToDosResponse> Handle(GetToDoListToDos request, CancellationToken cancellationToken)
        {
            var toDos = _repository.GetToDoListToDos(request.ToDoListId);

            return Task.FromResult(new GetToDoListToDosResponse {Data = toDos});
        }
    }
}