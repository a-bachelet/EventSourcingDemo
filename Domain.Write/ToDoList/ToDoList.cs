using System;
using System.Collections.Generic;
using Domain.Write.ToDoList.Event;

namespace Domain.Write.ToDoList
{
    public enum ToDoListCurrentState
    {
        Undefined = 0,
        Created = 1,
        Deleted = 2
    }
    
    public class ToDoListState : IAggregateState
    {
        public string Label { get; set; }
        
        public string Description { get; set; }
        
        public ToDoListCurrentState CurrentState { get; set; }
    }
    
    public class ToDoList : Aggregate
    {
        public override ToDoListState State { get; } = new();

        public ToDoList()
        {
        }

        public ToDoList(Guid id, long version, IList<IEvent> history) : base(id, version, history)
        {
        }
        
        public ToDoList(Guid id, long version, ToDoListState snapshot, IList<IEvent> history) : base(id, version, snapshot, history)
        {
        }
        
        public override void Hydrate(IAggregateState snapshot)
        {
            switch (snapshot)
            {
                case ToDoListState x:
                    State.Label = x.Label;
                    State.Description = x.Description;
                    State.CurrentState = x.CurrentState;
                    break;
                default :
                    throw new InvalidOperationException($"Unsupported snapshot for aggregate of type {nameof(ToDoList)}.");
            }
        }

        public override void When(IEvent @event, bool isNew = true)
        {
            switch (@event)
            {
                case ToDoListAddedV1 x:
                    Apply(x);
                    break;
                case ToDoListUpdatedV1 x:
                    Apply(x);
                    break;
                case ToDoListDeletedV1 x:
                    Apply(x);
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unsupported event of type {nameof(@event)} on aggregate of type {nameof(ToDoList)}.");
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

        private void Apply(ToDoListAddedV1 @event)
        {
            if (State.CurrentState != ToDoListCurrentState.Undefined)
                throw new InvalidOperationException($"Cannot add an already existing {nameof(ToDoList)}.");

            if (string.IsNullOrEmpty(@event.Data.Label))
                throw new InvalidOperationException($"{nameof(ToDoList)} label cannot be null or empty.");
            
            if (string.IsNullOrEmpty(@event.Data.Description))
                throw new InvalidOperationException($"{nameof(ToDoList)} description cannot be null or empty.");

            State.Label = @event.Data.Label;
            State.Description = @event.Data.Description;
        }
        
        private void Apply(ToDoListUpdatedV1 @event)
        {
            switch (State.CurrentState)
            {
                case ToDoListCurrentState.Undefined:
                    throw new InvalidOperationException($"Cannot update a non existing {nameof(ToDoList)}.");
                case ToDoListCurrentState.Deleted:
                    throw new InvalidOperationException($"Cannot update a deleted {nameof(ToDoList)}.");
            }

            if (string.IsNullOrEmpty(@event.Data.Label))
                throw new InvalidOperationException($"{nameof(ToDoList)} label cannot be null or empty.");
            
            if (string.IsNullOrEmpty(@event.Data.Description))
                throw new InvalidOperationException($"{nameof(ToDoList)} description cannot be null or empty.");

            State.Label = @event.Data.Label;
            State.Description = @event.Data.Description;
        }

        private void Apply(ToDoListDeletedV1 @event)
        {
            if (State.CurrentState == ToDoListCurrentState.Undefined)
                throw new InvalidOperationException($"Cannot delete a non existing {nameof(ToDoList)}.");

            State.CurrentState = ToDoListCurrentState.Deleted;
        }
    }
}