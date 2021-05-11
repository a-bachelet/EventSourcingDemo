using System;
using System.Collections.Generic;

namespace Domain
{
    #region Interfaces

    public interface IAggregateState
    {
    }
    
    public interface IAggregate
    {
        Guid Id { get; }
        
        long Version { get; }

        IAggregateState State { get; }

        IList<IEvent> GetCommittedEvents();

        IList<IEvent> GetUnCommittedEvents();

        void When(IEvent @event, bool isNew);
    }

    #endregion

    #region Classes

    public abstract class Aggregate : IAggregate
    {
        public Guid Id { get; }
        
        public long Version { get; }

        public abstract IAggregateState State { get; }

        protected IList<IEvent> CommittedEvents { get; } = new List<IEvent>();

        protected IList<IEvent> UnCommittedEvents { get; } = new List<IEvent>();

        protected Aggregate()
        {
            Id = Guid.NewGuid();
            Version = 1L;
        }

        protected Aggregate(Guid id, long version, IList<IEvent> history)
        {
            Id = id;
            Version = version + 1L;

            foreach (var @event in history)
            {
                When(@event, false);
            }
        }
        
        protected Aggregate(Guid id, long version, IAggregateState snapshot, IList<IEvent> history)
        {
            Id = id;
            Version = version + 1L;

            Hydrate(snapshot);
            
            foreach (var @event in history)
            {
                When(@event, false);
            }
        }
        
        public IList<IEvent> GetCommittedEvents()
        {
            return CommittedEvents;
        }

        public IList<IEvent> GetUnCommittedEvents()
        {
            return UnCommittedEvents;
        }

        public abstract void Hydrate(IAggregateState snapshot);
        
        public abstract void When(IEvent @event, bool isNew = true);
    }

    #endregion
}