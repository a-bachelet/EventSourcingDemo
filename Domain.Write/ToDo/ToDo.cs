using System;
using System.Collections.Generic;
using Domain.Write.ToDo.Event;

namespace Domain.Write.ToDo
{
    public enum ToDoCurrentState
    {
        Undefined = 0,
        Waiting   = 1,
        Started   = 2,
        Finished  = 3,
        Deleted   = 4
    }
    
    public class ToDoState : IAggregateState
    {
        public Guid ToDoListId;
        
        public string Label { get; set; }
        
        public string Description { get; set; }

        public ToDoCurrentState CurrentState { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime EndedAt { get; set; }
    }
    
    public class ToDo : Aggregate
    {
        public override ToDoState State { get; } = new();

        public ToDo()
        {
        }

        public ToDo(Guid id, long version, IList<IEvent> history) : base(id, version, history)
        {
        }

        public ToDo(Guid id, long version, ToDoState snapshot, IList<IEvent> history) : base(id, version, snapshot, history)
        {
        }

        public override void Hydrate(IAggregateState snapshot)
        {
            switch (snapshot)
            {
                case ToDoState x:
                    State.ToDoListId = x.ToDoListId;
                    State.Label = x.Label;
                    State.Description = x.Description;
                    State.CurrentState = x.CurrentState;
                    State.StartedAt = x.StartedAt;
                    State.EndedAt = x.EndedAt;
                    break;
                default :
                    throw new InvalidOperationException($"Unsupported snapshot for aggregate of type {nameof(ToDo)}.");
            }
        }

        public override void When(IEvent @event, bool isNew = true)
        {
            switch (@event)
            {
                case ToDoAddedV1 x:
                    Apply(x);
                    break;
                case ToDoUpdatedV1 x:
                    Apply(x);
                    break;
                case ToDoStartedV1 x:
                    Apply(x);
                    break;
                case ToDoFinishedV1 x:
                    Apply(x);
                    break;
                case ToDoResetedV1 x:
                    Apply(x);
                    break;
                case ToDoDeletedV1 x:
                    Apply(x);
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unsupported event of type {nameof(@event)} on aggregate of type {nameof(ToDo)}.");
            }

            if (isNew)
            {
                UnCommittedEvents.Add(@event);
            }
            else
            {
                CommittedEvents.Add(@event);
            }
        }

        private void Apply(ToDoAddedV1 @event)
        {
            if (State.CurrentState != ToDoCurrentState.Undefined)
                throw new InvalidOperationException(
                    $"Cannot add an already existing {nameof(ToDo)}.");

            if (string.IsNullOrEmpty(@event.Data.Label))
                throw new InvalidOperationException($"{nameof(ToDo)} label cannot be null or empty.");

            if (string.IsNullOrEmpty(@event.Data.Description))
                throw new InvalidOperationException($"{nameof(ToDo)} description cannot be null or empty.");

            State.ToDoListId = @event.Data.ToDoListId;
            State.CurrentState = ToDoCurrentState.Waiting;
            State.Label = @event.Data.Label;
            State.Description = @event.Data.Description;
        }
        
        private void Apply(ToDoUpdatedV1 @event)
        {
            switch (State.CurrentState)
            {
                case ToDoCurrentState.Undefined:
                    throw new InvalidOperationException(
                        $"Cannot update an non existing {nameof(ToDo)}.");
                case ToDoCurrentState.Deleted:
                    throw new InvalidOperationException(
                        $"Cannot update a deleted {nameof(ToDo)}.");
            }

            if (string.IsNullOrEmpty(@event.Data.Label))
                throw new InvalidOperationException($"{nameof(ToDo)} label cannot be null or empty.");

            if (string.IsNullOrEmpty(@event.Data.Description))
                throw new InvalidOperationException($"{nameof(ToDo)} description cannot be null or empty.");

            State.CurrentState = ToDoCurrentState.Waiting;
            State.Label = @event.Data.Label;
            State.Description = @event.Data.Description;
        }
        
        private void Apply(ToDoStartedV1 @event)
        {
            if (State.CurrentState != ToDoCurrentState.Waiting)
                throw new InvalidOperationException($"Cannot start a non waiting {nameof(ToDo)}.");

            State.CurrentState = ToDoCurrentState.Started;
            State.StartedAt = DateTime.Now;
        }
        
        private void Apply(ToDoFinishedV1 @event)
        {
            if (State.CurrentState != ToDoCurrentState.Started)
                throw new InvalidOperationException($"Cannot finish a non started {nameof(ToDo)}.");

            State.CurrentState = ToDoCurrentState.Finished;
            State.EndedAt = DateTime.Now;
        }

        private void Apply(ToDoResetedV1 @event)
        {
            switch (State.CurrentState)
            {
                case ToDoCurrentState.Undefined:
                    throw new InvalidOperationException($"Cannot reset a non existing {nameof(ToDo)}.");
                case ToDoCurrentState.Deleted:
                    throw new InvalidOperationException($"Cannot reset a deleted {nameof(ToDo)}.");
            }

            State.CurrentState = ToDoCurrentState.Waiting;
            State.StartedAt = DateTime.MinValue;
            State.EndedAt = DateTime.MinValue;
        }
        
        private void Apply(ToDoDeletedV1 @event)
        {
            if (State.CurrentState == ToDoCurrentState.Undefined)
                throw new InvalidOperationException($"Cannot delete a non existing {nameof(ToDo)}.");

            State.CurrentState = ToDoCurrentState.Deleted;
        }
    }
}