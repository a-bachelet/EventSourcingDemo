using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Write.ToDo.Event;
using Domain.Write.ToDoList.Event;
using MediatR;

namespace Domain.Read.ToDo.Projection
{
    public interface IToDoProjection : IProjection
    {
        Entity.ToDo GetToDo(Guid toDoId);
    }
    
    public class ToDoProjection : IToDoProjection,
        INotificationHandler<ToDoAddedV1>,
        INotificationHandler<ToDoUpdatedV1>,
        INotificationHandler<ToDoStartedV1>,
        INotificationHandler<ToDoFinishedV1>,
        INotificationHandler<ToDoResetedV1>,
        INotificationHandler<ToDoDeletedV1>,
        INotificationHandler<ToDoListDeletedV1>
    {
        private readonly IToDoProjectionRepository _repository;
        
        public ToDoProjection(IToDoProjectionRepository repository)
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
                case ToDoListDeletedV1 x:
                    _repository.OnToDoListDeleted(x);
                    break;
            }
        }

        public Entity.ToDo GetToDo(Guid toDoId) => _repository.GetToDo(toDoId);

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

        public Task Handle(ToDoListDeletedV1 notification, CancellationToken cancellationToken)
        {
            When(notification);

            return Task.CompletedTask;
        }
    }
}