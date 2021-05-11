using System;
using Domain;

namespace Infrastructure.InMemory.Common
{
    public class InMemoryStoredEvent
    {
        public IEvent Event { get; set; }

        public DateTime StoredAt { get; set; }
        
        public long AggregateVersion { get; set; }
    }
}