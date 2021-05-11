using System;

namespace Infrastructure.InMemory.Common
{
    public class InMemoryStoredState
    {
        public string State { get; set; }

        public DateTime StoredAt { get; set; }
        
        public long AggregateVersion { get; set; }
    }
}