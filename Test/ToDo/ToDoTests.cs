using System;
using System.Collections.Generic;
using Domain;
using Domain.Write.ToDo;
using Domain.Write.ToDo.Event;
using Xunit;

namespace Test.ToDo
{
    public class ToDoTests
    {
        #region ToDoAdded
        
        [Fact]
        public void ItShouldAddAToDo()
        {
            var toDo = new Domain.Write.ToDo.ToDo();

            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = "My awesome ToDo description";
            
            var @event = new ToDoAddedV1(toDo.Id, toDoListId, label, description);
            
            toDo.When(@event);
            
            Assert.Equal(toDoListId, toDo.State.ToDoListId);
            Assert.Equal(label, toDo.State.Label);
            Assert.Equal(description, toDo.State.Description);
            Assert.Equal(ToDoCurrentState.Waiting, toDo.State.CurrentState);
        }

        [Fact]
        public void ItShouldNotAddAToDoIfTheToDoAlreadyExists()
        {
            var toDoId = Guid.NewGuid();
            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = "My awesome ToDo description";

            var history = new List<IEvent>
            {
                new ToDoAddedV1(toDoId, toDoListId, label, description)
            };
         
            var toDo = new Domain.Write.ToDo.ToDo(toDoId, history.Count + 1, history);
            
            var @event = new ToDoAddedV1(toDo.Id, toDoListId, label, description);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }
        
        [Fact]
        public void ItShouldNotAddAToDoWithANullLabel()
        {
            var toDo = new Domain.Write.ToDo.ToDo();

            var toDoListId = Guid.NewGuid();
            const string label = null;
            const string description = "My awesome ToDo description";

            var @event = new ToDoAddedV1(toDo.Id, toDoListId, label, description);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }
        
        [Fact]
        public void ItShouldNotAddAToDoWithANullDescription()
        {
            var toDo = new Domain.Write.ToDo.ToDo();

            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = null;

            var @event = new ToDoAddedV1(toDo.Id, toDoListId, label, description);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }

        #endregion
        
        #region ToDoUpdated
        
        [Fact]
        public void ItShouldUpdateTheToDo()
        {
            var toDoId = Guid.NewGuid();
            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = "My awesome ToDo description";
            const string newLabel = "My awesome new ToDo label";
            const string newDescription = "My awesome new ToDo description";
            
            var history = new List<IEvent>
            {
                new ToDoAddedV1(toDoId, toDoListId, label, description)
            };
            
            var toDo = new Domain.Write.ToDo.ToDo(toDoId, history.Count + 1, history);
            
            var @event = new ToDoUpdatedV1(toDo.Id, toDoListId, newLabel, newDescription);
            
            toDo.When(@event);
            
            Assert.Equal(newLabel, toDo.State.Label);
            Assert.Equal(newDescription, toDo.State.Description);
        }

        [Fact]
        public void ItShouldNotUpdateTheToDoIfTheToDoDoesntExist()
        {
            var toDoListId = Guid.NewGuid();
            const string newLabel = "My awesome new ToDo label";
            const string newDescription = "My awesome new ToDo description";
            
            var toDo = new Domain.Write.ToDo.ToDo();
            
            var @event = new ToDoUpdatedV1(toDo.Id, toDoListId, newLabel, newDescription);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }
        
        [Fact]
        public void ItShouldNotUpdateTheToDoIfTheToDoHasBeenDeleted()
        {
            var toDoId = Guid.NewGuid();
            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = "My awesome ToDo description";
            const string newLabel = "My awesome new ToDo label";
            const string newDescription = "My awesome new ToDo description";
            
            var history = new List<IEvent>
            {
                new ToDoAddedV1(toDoId, toDoListId, label, description),
                new ToDoDeletedV1(toDoId, toDoListId)
            };
            
            var toDo = new Domain.Write.ToDo.ToDo(toDoId, history.Count + 1, history);
            
            var @event = new ToDoUpdatedV1(toDo.Id, toDoListId, newLabel, newDescription);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }

        [Fact]
        public void ItShouldNotUpdateTheToDoWithANullLabel()
        {
            var toDoId = Guid.NewGuid();
            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = "My awesome ToDo description";
            const string newLabel = null;
            const string newDescription = "My awesome new ToDo description";
            
            var history = new List<IEvent>
            {
                new ToDoAddedV1(toDoId, toDoListId, label, description)
            };
            
            var toDo = new Domain.Write.ToDo.ToDo(toDoId, history.Count + 1, history);
            
            var @event = new ToDoUpdatedV1(toDo.Id, toDoListId, newLabel, newDescription);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }
        
        [Fact]
        public void ItShouldNotUpdateTheToDoWithANullDescription()
        {
            var toDoId = Guid.NewGuid();
            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = "My awesome ToDo description";
            const string newLabel = "My awesome new ToDo label";
            const string newDescription = null;
            
            var history = new List<IEvent>
            {
                new ToDoAddedV1(toDoId, toDoListId, label, description)
            };
            
            var toDo = new Domain.Write.ToDo.ToDo(toDoId, history.Count + 1, history);
            
            var @event = new ToDoUpdatedV1(toDo.Id, toDoListId, newLabel, newDescription);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }
        
        #endregion

        #region ToDoStarted
        
        [Fact]
        public void ItShouldStartTheToDo()
        {
            var toDoId = Guid.NewGuid();
            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = "My awesome ToDo description";
            const string newLabel = "My awesome new ToDo label";
            const string newDescription = "My awesome new ToDo description";
            
            var history = new List<IEvent>
            {
                new ToDoAddedV1(toDoId, toDoListId, label, description),
                new ToDoUpdatedV1(toDoId, toDoListId, newLabel, newDescription)
            };
            
            var toDo = new Domain.Write.ToDo.ToDo(toDoId, history.Count + 1, history);
            
            var @event = new ToDoStartedV1(toDo.Id, toDoListId);
            
            toDo.When(@event);
            
            Assert.Equal(ToDoCurrentState.Started, toDo.State.CurrentState);
            Assert.NotEqual(DateTime.MinValue, toDo.State.StartedAt);
        }

        [Fact]
        public void ItShouldNotStartTheToDoIfTheToDoIsNotWaiting()
        {
            var toDoListId = Guid.NewGuid();
            
            var toDo = new Domain.Write.ToDo.ToDo();

            var @event = new ToDoStartedV1(toDo.Id, toDoListId);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }
        
        #endregion
        
        #region ToDoFinished
        
        [Fact]
        public void ItShouldFinishTheToDo()
        {
            var toDoId = Guid.NewGuid();
            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = "My awesome ToDo description";
            const string newLabel = "My awesome new ToDo label";
            const string newDescription = "My awesome new ToDo description";
            
            var history = new List<IEvent>
            {
                new ToDoAddedV1(toDoId, toDoListId, label, description),
                new ToDoUpdatedV1(toDoId, toDoListId, newLabel, newDescription),
                new ToDoStartedV1(toDoId, toDoListId)
            };
            
            var toDo = new Domain.Write.ToDo.ToDo(toDoId, history.Count + 1, history);
            
            var @event = new ToDoFinishedV1(toDo.Id, toDoListId);
            
            toDo.When(@event);
            
            Assert.Equal(ToDoCurrentState.Finished, toDo.State.CurrentState);
            Assert.NotEqual(DateTime.MinValue, toDo.State.EndedAt);
        }
        
        [Fact]
        public void ItShouldNotFinishTheToDoIfTheToDoIsNotStarted()
        {
            var toDoListId = Guid.NewGuid();
            
            var toDo = new Domain.Write.ToDo.ToDo();

            var @event = new ToDoFinishedV1(toDo.Id, toDoListId);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }

        #endregion
        
        #region ToDoReseted
        
        [Fact]
        public void ItShouldResetTheToDo()
        {
            var toDoId = Guid.NewGuid();
            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = "My awesome ToDo description";
            const string newLabel = "My awesome new ToDo label";
            const string newDescription = "My awesome new ToDo description";
            
            var history = new List<IEvent>
            {
                new ToDoAddedV1(toDoId, toDoListId, label, description),
                new ToDoUpdatedV1(toDoId, toDoListId, newLabel, newDescription),
                new ToDoStartedV1(toDoId, toDoListId),
                new ToDoFinishedV1(toDoId, toDoListId)
            };
            
            var toDo = new Domain.Write.ToDo.ToDo(toDoId, history.Count + 1, history);
            
            var @event = new ToDoResetedV1(toDo.Id, toDoListId);
            
            toDo.When(@event);
            
            Assert.Equal(ToDoCurrentState.Waiting, toDo.State.CurrentState);
            Assert.Equal(DateTime.MinValue, toDo.State.StartedAt);
            Assert.Equal(DateTime.MinValue, toDo.State.EndedAt);
        }
        
        [Fact]
        public void ItShouldNotResetTheToDoIfTheToDoDoesntExist()
        {
            var toDoListId = Guid.NewGuid();
            
            var toDo = new Domain.Write.ToDo.ToDo();

            var @event = new ToDoResetedV1(toDo.Id, toDoListId);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }
        
        [Fact]
        public void ItShouldNotResetTheToDoIfTheToDoIsDeleted()
        {
            var toDoId = Guid.NewGuid();
            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = "My awesome ToDo description";
            
            var history = new List<IEvent>
            {
                new ToDoAddedV1(toDoId, toDoListId, label, description),
                new ToDoDeletedV1(toDoId, toDoListId)
            };
            
            var toDo = new Domain.Write.ToDo.ToDo(toDoId, history.Count + 1, history);

            var @event = new ToDoResetedV1(toDo.Id, toDoListId);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }
        
        #endregion
        
        #region ToDoDeleted

        [Fact]
        public void ItShouldDeleteTheToDo()
        {
            var toDoId = Guid.NewGuid();
            var toDoListId = Guid.NewGuid();
            const string label = "My awesome ToDo label";
            const string description = "My awesome ToDo description";
            const string newLabel = "My awesome new ToDo label";
            const string newDescription = "My awesome new ToDo description";
            
            var history = new List<IEvent>
            {
                new ToDoAddedV1(toDoId, Guid.Empty, label, description),
                new ToDoUpdatedV1(toDoId, toDoListId, newLabel, newDescription),
                new ToDoStartedV1(toDoId, toDoListId),
                new ToDoFinishedV1(toDoId, toDoListId),
                new ToDoResetedV1(toDoId, toDoListId)
            };
            
            var toDo = new Domain.Write.ToDo.ToDo(toDoId, history.Count + 1, history);
            
            var @event = new ToDoDeletedV1(toDo.Id, toDoListId);
            
            toDo.When(@event);
            
            Assert.Equal(ToDoCurrentState.Deleted, toDo.State.CurrentState);
        }
        
        [Fact]
        public void ItShouldNotDeleteTheToDoIfTheToDoDoesntExist()
        {
            var toDo = new Domain.Write.ToDo.ToDo();
            
            var toDoListId = Guid.NewGuid();
            
            var @event = new ToDoDeletedV1(toDo.Id, toDoListId);

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }
        
        #endregion
        
        #region When

        private class MyCustomEventData : EventData
        {
        }
        
        private class MyCustomEvent : Event
        {
            public MyCustomEvent(Guid aggregateId, MyCustomEventData data) : base(aggregateId, 1, "MyCustomEvent")
            {
                Data = data;
            }

            public override MyCustomEventData Data { get; }
        }
        
        [Fact]
        public void ItShouldNotApplyAnUnsupportedEvent()
        {
            var toDo = new Domain.Write.ToDo.ToDo();

            var @event = new MyCustomEvent(toDo.Id, new MyCustomEventData());

            Assert.Throws<InvalidOperationException>(() => toDo.When(@event));
        }
        
        #endregion When
        
        #region Hydrate

        private class MyCustomSnapshot : IAggregateState
        {
        }
        
        [Fact]
        public void ItShouldRestoreTheToDoStateFromASnapshot()
        {
            const string label = "My label";
            const string description = "My description";
            const ToDoCurrentState currentState = ToDoCurrentState.Waiting;
            var startedAt = DateTime.Now;
            var endedAt = DateTime.Now;
            var snapshot = new ToDoState
            {
                Label = label,
                Description = description,
                CurrentState = currentState,
                StartedAt = startedAt,
                EndedAt = endedAt
            };
            var history = new List<IEvent>();
            var todoId = Guid.NewGuid();

            var toDo = new Domain.Write.ToDo.ToDo(todoId, history.Count + 1, snapshot, history);
            
            Assert.Equal(label, toDo.State.Label);
            Assert.Equal(description, toDo.State.Description);
            Assert.Equal(currentState, toDo.State.CurrentState);
            Assert.Equal(startedAt, toDo.State.StartedAt);
            Assert.Equal(endedAt, toDo.State.EndedAt);
        }
        
        [Fact]
        public void ItShouldNotHydrateTheToDoStateFromAnUnsupportedSnapshot()
        {
            var snapshot = new MyCustomSnapshot();
            var history = new List<IEvent>();
            var todoId = Guid.NewGuid();

            var toDo = new Domain.Write.ToDo.ToDo(todoId, history.Count + 1, history);

            Assert.Throws<InvalidOperationException>(() => toDo.Hydrate(snapshot));
        }
        
        #endregion
    }
}