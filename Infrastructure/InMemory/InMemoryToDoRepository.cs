using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Write.ToDo;
using Infrastructure.InMemory.Common;
using MediatR;
using Newtonsoft.Json;

namespace Infrastructure.InMemory
{
    public class InMemoryToDoRepository : IAggregateRepository<ToDo>
    {
        private readonly IMediator _mediator;
        
        private readonly Dictionary<string, IList<InMemoryStoredEvent>> _aggregates = new();

        private readonly Dictionary<string, IList<InMemoryStoredState>> _snapshots = new();

        public InMemoryToDoRepository(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task SaveAsync(ToDo aggregate)
        {
            if (!_aggregates.ContainsKey($"{nameof(ToDo)}-{aggregate.Id}"))
            {
                _aggregates[$"{nameof(ToDo)}-{aggregate.Id}"] = new List<InMemoryStoredEvent>();
            }
            
            foreach (var @event in aggregate.GetUnCommittedEvents())
            {
                _aggregates[$"{nameof(ToDo)}-{aggregate.Id}"]
                    .Add(new InMemoryStoredEvent
                    {
                        Event = @event,
                        StoredAt = DateTime.Now,
                        AggregateVersion = aggregate.Version
                    });
                    
                aggregate.GetCommittedEvents().Add(@event);

                await _mediator.Publish(@event);

                if (_aggregates[$"{nameof(ToDo)}-{aggregate.Id}"].Count % 2 != 0) continue;
                
                if (!_snapshots.ContainsKey($"{nameof(ToDo)}-{aggregate.Id}"))
                {
                    _snapshots[$"{nameof(ToDo)}-{aggregate.Id}"] = new List<InMemoryStoredState>();
                }

                _snapshots[$"{nameof(ToDo)}-{aggregate.Id}"].Add(new InMemoryStoredState
                {
                    State = JsonConvert.SerializeObject(aggregate.State),
                    StoredAt = DateTime.Now,
                    AggregateVersion = aggregate.Version
                });
            }
                
            aggregate.GetUnCommittedEvents().Clear();

            await Task.CompletedTask;
        }

        public async Task<ToDo> LoadAsync(Guid? aggregateId)
        {
            if (aggregateId == null || !_aggregates.ContainsKey($"{nameof(ToDo)}-{aggregateId}"))
            {
                return await Task.FromResult(new ToDo());
            }

            if (!_snapshots.ContainsKey($"{nameof(ToDo)}-{aggregateId}"))
                return await Task.FromResult(new ToDo((Guid) aggregateId,
                    _aggregates[$"{nameof(ToDo)}-{aggregateId}"].Last().AggregateVersion,
                    _aggregates[$"{nameof(ToDo)}-{aggregateId}"]
                        .Select(se => se.Event)
                        .ToList()));
            {
                var snapshot = _snapshots[$"{nameof(ToDo)}-{aggregateId}"].Last();
                var events = _aggregates[$"{nameof(ToDo)}-{aggregateId}"]
                    .Where(e => e.AggregateVersion > snapshot.AggregateVersion)
                    .ToList();

                return await Task.FromResult(new ToDo(
                    (Guid) aggregateId,
                    events.LastOrDefault() != null ? events.Last().AggregateVersion : snapshot.AggregateVersion,
                    JsonConvert.DeserializeObject<ToDoState>(snapshot.State),
                    events.Select(se => se.Event).ToList()
                ));
            }

        }
    }
}