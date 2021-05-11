using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Read.ToDo.Projection;
using MediatR;

namespace Domain.Read.ToDo.Query
{
    public class GetToDoResponse
    {
        public Entity.ToDo Data { get; set; }
    }
    
    public class GetToDo : IRequest<GetToDoResponse>
    {
        public Guid ToDoId { get; set; }
    }
    
    public class GetToDoHandler : IRequestHandler<GetToDo, GetToDoResponse>
    {
        private readonly IToDoProjectionRepository _repository;
        
        public GetToDoHandler(IToDoProjectionRepository repository)
        {
            _repository = repository;
        }
        
        public Task<GetToDoResponse> Handle(GetToDo request, CancellationToken cancellationToken)
        {
            var toDo = _repository.GetToDo(request.ToDoId);
            
            return Task.FromResult(new GetToDoResponse { Data = toDo });
        }
    }
}