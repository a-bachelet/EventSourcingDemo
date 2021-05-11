using System;
using System.Threading.Tasks;

namespace Domain
{
    #region Interfaces

    public interface IAggregateRepository<TAggregate> where TAggregate : IAggregate
    {
        Task SaveAsync(TAggregate aggregate);

        Task<TAggregate> LoadAsync(Guid? aggregateId);
    }
    
    #endregion
}