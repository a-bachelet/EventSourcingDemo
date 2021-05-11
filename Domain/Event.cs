using System;
using MediatR;

namespace Domain
{
    #region Interfaces
     
    public interface IEventData
    {
    }
    
    public interface IEvent : INotification
    {
        Guid AggregateId { get; }
        
        long Version { get; }
        
        string Name { get; }

        IEventData Data { get; }
    }

    #endregion
        
    #region Classes
    
    public abstract class EventData : IEventData
    {
    }
        
    public abstract class Event : IEvent
    {
        public Guid AggregateId { get; }
        
        public long Version { get; }
        
        public string Name { get; }

        public abstract IEventData Data { get; }

        protected Event(Guid aggregateId, long version, string name)
        {
            AggregateId = aggregateId;
            Version = version;
            Name = name;
        }
    }

    #endregion
}