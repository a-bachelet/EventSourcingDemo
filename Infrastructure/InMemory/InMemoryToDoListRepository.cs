using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Write.ToDoList;
using Infrastructure.InMemory.Common;
using MediatR;
using Newtonsoft.Json;

namespace Infrastructure.InMemory
{
    public class InMemoryToDoListRepository : IAggregateRepository<ToDoList>
    {
        private readonly IMediator _mediator;
        
        private readonly Dictionary<string, IList<InMemoryStoredEvent>> _aggregates = new();

        private readonly Dictionary<string, IList<InMemoryStoredState>> _snapshots = new();

        public InMemoryToDoListRepository(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task SaveAsync(ToDoList aggregate)
        {
            if (!_aggregates.ContainsKey($"{nameof(ToDoList)}-{aggregate.Id}"))
            {
                _aggregates[$"{nameof(ToDoList)}-{aggregate.Id}"] = new List<InMemoryStoredEvent>();
            }
            
            foreach (var @event in aggregate.GetUnCommittedEvents())
            {
                _aggregates[$"{nameof(ToDoList)}-{aggregate.Id}"]
                    .Add(new InMemoryStoredEvent
                    {
                        Event = @event,
                        StoredAt = DateTime.Now,
                        AggregateVersion = aggregate.Version
                    });
                    
                aggregate.GetCommittedEvents().Add(@event);

                await _mediator.Publish(@event);

                if (_aggregates[$"{nameof(ToDoList)}-{aggregate.Id}"].Count % 2 != 0) continue;
                
                if (!_snapshots.ContainsKey($"{nameof(ToDoList)}-{aggregate.Id}"))
                {
                    _snapshots[$"{nameof(ToDoList)}-{aggregate.Id}"] = new List<InMemoryStoredState>();
                }

                _snapshots[$"{nameof(ToDoList)}-{aggregate.Id}"].Add(new InMemoryStoredState
                {
                    State = JsonConvert.SerializeObject(aggregate.State),
                    StoredAt = DateTime.Now,
                    AggregateVersion = aggregate.Version
                });
            }
                
            aggregate.GetUnCommittedEvents().Clear();

            await Task.CompletedTask;
        }

        public async Task<ToDoList> LoadAsync(Guid? aggregateId)
        {
            if (aggregateId == null || !_aggregates.ContainsKey($"{nameof(ToDoList)}-{aggregateId}"))
            {
                return await Task.FromResult(new ToDoList());
            }

            if (!_snapshots.ContainsKey($"{nameof(ToDoList)}-{aggregateId}"))
                return await Task.FromResult(new ToDoList((Guid) aggregateId,
                    _aggregates[$"{nameof(ToDoList)}-{aggregateId}"].Last().AggregateVersion,
                    _aggregates[$"{nameof(ToDoList)}-{aggregateId}"]
                        .Select(se => se.Event)
                        .ToList()));
            {
                var snapshot = _snapshots[$"{nameof(ToDoList)}-{aggregateId}"].Last();
                var events = _aggregates[$"{nameof(ToDoList)}-{aggregateId}"]
                    .Where(e => e.AggregateVersion > snapshot.AggregateVersion)
                    .ToList();

                return await Task.FromResult(new ToDoList(
                    (Guid) aggregateId,
                    events.LastOrDefault() != null ? events.Last().AggregateVersion : snapshot.AggregateVersion,
                    JsonConvert.DeserializeObject<ToDoListState>(snapshot.State),
                    events.Select(se => se.Event).ToList()
                ));
            }

        }
    }
}