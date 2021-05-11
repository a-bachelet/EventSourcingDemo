using System;
using System.Linq;
using Domain.Write.ToDo.Event;
using Domain.Write.ToDoList.Event;

namespace Domain.Read.ToDo.Projection
{
    public interface IToDoProjectionRepository
    {
        Entity.ToDo GetToDo(Guid toDoId);

        void OnToDoAdded(ToDoAddedV1 @event);

        void OnToDoUpdated(ToDoUpdatedV1 @event);

        void OnToDoStarted(ToDoStartedV1 @event);

        void OnToDoFinished(ToDoFinishedV1 @event);

        void OnToDoReseted(ToDoResetedV1 @event);

        void OnToDoDeleted(ToDoDeletedV1 @event);

        void OnToDoListDeleted(ToDoListDeletedV1 @event);
    }
}