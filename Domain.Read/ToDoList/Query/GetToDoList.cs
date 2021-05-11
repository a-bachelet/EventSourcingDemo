using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Read.ToDoList.Projection;
using MediatR;

namespace Domain.Read.ToDoList.Query
{
    public class GetToDoListResponse
    {
        public Entity.ToDoList Data { get; set; }
    }
    
    public class GetToDoList : IRequest<GetToDoListResponse>
    {
        public Guid ToDoListId { get; set; }
    }
    
    public class GetToDoListHandler : IRequestHandler<GetToDoList, GetToDoListResponse>
    {
        private readonly IToDoListProjectionRepository _repository;
        
        public GetToDoListHandler(IToDoListProjectionRepository repository)
        {
            _repository = repository;
        }
        
        public Task<GetToDoListResponse> Handle(GetToDoList request, CancellationToken cancellationToken)
        {
            var toDo = _repository.GetToDoList(request.ToDoListId);

            return Task.FromResult(new GetToDoListResponse { Data = toDo });
        }
    }
}