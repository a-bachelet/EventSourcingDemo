using System.Collections.Generic;

namespace Domain
{
    public interface IProjection
    {
        void Rebuild(IList<IEvent> history);

        void When(IEvent @event);
    }

    public abstract class Projection : IProjection
    {
        public void Rebuild(IList<IEvent> history)
        {
            foreach (var @event in history)
            {
                When(@event);
            }
        }

        public abstract void When(IEvent @event);
    }
}