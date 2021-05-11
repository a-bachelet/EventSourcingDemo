using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Read.ToDo.Entity;
using Domain.Read.ToDo.Projection;
using Domain.Write.ToDo.Event;
using Domain.Write.ToDoList.Event;

namespace Infrastructure.InMemory
{
    public class InMemoryToDoProjectionRepository : IToDoProjectionRepository
    {
        private readonly ICollection<ToDo> _data = new List<ToDo>();

        public ToDo GetToDo(Guid toDoId) => _data.FirstOrDefault(t => t.Id == toDoId);
        
        public void OnToDoAdded(ToDoAddedV1 @event)
        {
            _data.Add(new ToDo
            {
                Id = @event.AggregateId,
                ToDoListId = @event.Data.ToDoListId,
                Label = @event.Data.Label,
                Description = @event.Data.Description
            });
        }

        public void OnToDoUpdated(ToDoUpdatedV1 @event)
        {
            var toDo = _data.First(t => t.Id == @event.AggregateId);

            toDo.State = ToDoState.Waiting;
            toDo.Label = @event.Data.Label;
            toDo.Description = @event.Data.Description;
        }

        public void OnToDoStarted(ToDoStartedV1 @event)
        {
            var toDo = _data.First(t => t.Id == @event.AggregateId);

            toDo.State = ToDoState.Started;
            toDo.StartedAt = DateTime.Now;
        }

        public void OnToDoFinished(ToDoFinishedV1 @event)
        {
            var toDo = _data.First(t => t.Id == @event.AggregateId);

            toDo.State = ToDoState.Finished;
            toDo.EndedAt = DateTime.Now;
        }

        public void OnToDoReseted(ToDoResetedV1 @event)
        {
            var toDo = _data.First(t => t.Id == @event.AggregateId);

            toDo.State = ToDoState.Waiting;
            toDo.StartedAt = DateTime.MinValue;
            toDo.EndedAt = DateTime.MinValue;
        }

        public void OnToDoDeleted(ToDoDeletedV1 @event)
        {
            var toDo = _data.First(t => t.Id == @event.AggregateId);

            _data.Remove(toDo);
        }

        public void OnToDoListDeleted(ToDoListDeletedV1 @event)
        {
            var toDos = _data.Where(t => t.ToDoListId == @event.AggregateId).ToList();

            foreach (var toDo in toDos)
            {
                _data.Remove(toDo);
            }
        }
    }
}