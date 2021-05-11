using System;
using System.Linq;
using Domain.Write.ToDo.Event;
using Domain.Write.ToDoList.Event;

namespace Domain.Read.ToDoList.Projection
{
    public interface IToDoListProjectionRepository
    {
        IQueryable<Entity.ToDoList> GetToDoLists();

        Entity.ToDoList GetToDoList(Guid toDoListId);

        IQueryable<ToDo.Entity.ToDo> GetToDoListToDos(Guid toDoListId);

        void OnToDoListAdded(ToDoListAddedV1 @event);

        void OnToDoListUpdated(ToDoListUpdatedV1 @event);

        void OnToDoListDeleted(ToDoListDeletedV1 @event);
        
        void OnToDoAdded(ToDoAddedV1 @event);

        void OnToDoUpdated(ToDoUpdatedV1 @event);

        void OnToDoStarted(ToDoStartedV1 @event);

        void OnToDoFinished(ToDoFinishedV1 @event);

        void OnToDoReseted(ToDoResetedV1 @event);

        void OnToDoDeleted(ToDoDeletedV1 @event);
    }
}