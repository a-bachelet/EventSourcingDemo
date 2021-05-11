using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Read.ToDo.Entity;
using Domain.Read.ToDoList.Entity;
using Domain.Read.ToDoList.Projection;
using Domain.Write.ToDo.Event;
using Domain.Write.ToDoList.Event;

namespace Infrastructure.InMemory
{
    public class InMemoryToDoListProjectionRepository : IToDoListProjectionRepository
    {
        private readonly ICollection<ToDoList> _toDoListsData = new List<ToDoList>();

        private readonly Dictionary<Guid, ICollection<ToDo>> _toDosData = new();

        public IQueryable<ToDoList> GetToDoLists() => _toDoListsData.AsQueryable();

        public ToDoList GetToDoList(Guid toDoListId) => _toDoListsData.FirstOrDefault(tl => tl.Id == toDoListId);

        public IQueryable<ToDo> GetToDoListToDos(Guid toDoListId) => _toDosData[toDoListId].AsQueryable();

        public void OnToDoListAdded(ToDoListAddedV1 @event)
        {
            var toDoList = new ToDoList
            {
                Id = @event.AggregateId,
                Label = @event.Data.Label,
                Description = @event.Data.Description
            };

            _toDoListsData.Add(toDoList);
            _toDosData.Add(toDoList.Id, new List<ToDo>());
        }

        public void OnToDoListUpdated(ToDoListUpdatedV1 @event)
        {
            var toDoList = _toDoListsData.First(tl => tl.Id == @event.AggregateId);

            toDoList.Label = @event.Data.Label;
            toDoList.Description = @event.Data.Description;
        }

        public void OnToDoListDeleted(ToDoListDeletedV1 @event)
        {
            var toDoList = _toDoListsData.First(tl => tl.Id == @event.AggregateId);

            _toDoListsData.Remove(toDoList);
            _toDosData.Remove(toDoList.Id);
        }

        public void OnToDoAdded(ToDoAddedV1 @event)
        {
            _toDosData[@event.Data.ToDoListId].Add(new ToDo
            {
                Id = @event.AggregateId,
                ToDoListId = @event.Data.ToDoListId,
                Label = @event.Data.Label,
                Description = @event.Data.Description
            });
        }

        public void OnToDoUpdated(ToDoUpdatedV1 @event)
        {
            var toDo = _toDosData[@event.Data.ToDoListId].First(t => t.Id == @event.AggregateId);

            toDo.State = ToDoState.Waiting;
            toDo.Label = @event.Data.Label;
            toDo.Description = @event.Data.Description;
        }

        public void OnToDoStarted(ToDoStartedV1 @event)
        {
            var toDo = _toDosData[@event.Data.ToDoListId].First(t => t.Id == @event.AggregateId);

            toDo.State = ToDoState.Started;
            toDo.StartedAt = DateTime.Now;
        }

        public void OnToDoFinished(ToDoFinishedV1 @event)
        {
            var toDo = _toDosData[@event.Data.ToDoListId].First(t => t.Id == @event.AggregateId);

            toDo.State = ToDoState.Finished;
            toDo.EndedAt = DateTime.Now;
        }

        public void OnToDoReseted(ToDoResetedV1 @event)
        {
            var toDo = _toDosData[@event.Data.ToDoListId].First(t => t.Id == @event.AggregateId);

            toDo.State = ToDoState.Waiting;
            toDo.StartedAt = DateTime.MinValue;
            toDo.EndedAt = DateTime.MinValue;
        }

        public void OnToDoDeleted(ToDoDeletedV1 @event)
        {
            var toDo = _toDosData[@event.Data.ToDoListId].First(t => t.Id == @event.AggregateId);

            _toDosData[@event.Data.ToDoListId].Remove(toDo);
        }
    }
}