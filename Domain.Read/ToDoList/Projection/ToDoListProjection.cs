using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Write.ToDo.Event;
using Domain.Write.ToDoList.Event;
using MediatR;

namespace Domain.Read.ToDoList.Projection
{
    public interface IToDoListProjection : IProjection
    {
        IQueryable<Entity.ToDoList> GetToDoLists();

        Entity.ToDoList GetToDoList(Guid toDoListId);

        IQueryable<ToDo.Entity.ToDo> GetToDoListToDos(Guid toDoListId);
    }
    
    public class ToDoListProjection : IToDoListProjection,
        INotificationHandler<ToDoListAddedV1>,
        INotificationHandler<ToDoListUpdatedV1>,
        INotificationHandler<ToDoListDeletedV1>,
        INotificationHandler<ToDoAddedV1>,
        INotificationHandler<ToDoUpdatedV1>,
        INotificationHandler<ToDoStartedV1>,
        INotificationHandler<ToDoFinishedV1>,
        INotificationHandler<ToDoResetedV1>,
        INotificationHandler<ToDoDeletedV1>
    {
        private readonly IToDoListProjectionRepository _repository;
        
        public ToDoListProjection(IToDoListProjectionRepository repository)
        {
            _repository = repository;
        }
        
        public void Rebuild(IList<IEvent> history)
        {
            foreach (var @event in history)
            {
                When(@event);
            }
        }

        public void When(IEvent @event)
        {
            switch (@event)
            {
                case ToDoListAddedV1 x:
                    _repository.OnToDoListAdded(x);
                    break;
                case ToDoListUpdatedV1 x:
                    _repository.OnToDoListUpdated(x);
                    break;
                case ToDoListDeletedV1 x:
                    _repository.OnToDoListDeleted(x);
                    break;
                case ToDoAddedV1 x:
                    _repository.OnToDoAdded(x);
                    break;
                case ToDoUpdatedV1 x:
                    _repository.OnToDoUpdated(x);
                    break;
                case ToDoStartedV1 x:
                    _repository.OnToDoStarted(x);
                    break;
                case ToDoFinishedV1 x:
                    _repository.OnToDoFinished(x);
                    break;
                case ToDoResetedV1 x:
                    _repository.OnToDoReseted(x);
                    break;
                case ToDoDeletedV1 x:
                    _repository.OnToDoDeleted(x);
                    break;
            }
        }

        public IQueryable<Entity.ToDoList> GetToDoLists() => _repository.GetToDoLists();

        public Entity.ToDoList GetToDoList(Guid toDoListId) => _repository.GetToDoList(toDoListId);

        public IQueryable<ToDo.Entity.ToDo> GetToDoListToDos(Guid toDoListId) => _repository.GetToDoListToDos(toDoListId);
        
        public Task Handle(ToDoListAddedV1 notification, CancellationToken cancellationToken)
        {
            When(notification);
            
            return Task.CompletedTask;
        }

        public Task Handle(ToDoListUpdatedV1 notification, CancellationToken cancellationToken)
        {
            When(notification);
            
            return Task.CompletedTask;
        }

        public Task Handle(ToDoListDeletedV1 notification, CancellationToken cancellationToken)
        {
            When(notification);
            
            return Task.CompletedTask;
        }

        public Task Handle(ToDoAddedV1 notification, CancellationToken cancellationToken)
        {
            When(notification);
            
            return Task.CompletedTask;
        }

        public Task Handle(ToDoUpdatedV1 notification, CancellationToken cancellationToken)
        {
            When(notification);
            
            return Task.CompletedTask;
        }

        public Task Handle(ToDoStartedV1 notification, CancellationToken cancellationToken)
        {
            When(notification);
            
            return Task.CompletedTask;
        }

        public Task Handle(ToDoFinishedV1 notification, CancellationToken cancellationToken)
        {
            When(notification);
            
            return Task.CompletedTask;
        }

        public Task Handle(ToDoResetedV1 notification, CancellationToken cancellationToken)
        {
            When(notification);
            
            return Task.CompletedTask;
        }

        public Task Handle(ToDoDeletedV1 notification, CancellationToken cancellationToken)
        {
            When(notification);
            
            return Task.CompletedTask;
        }
    }
}