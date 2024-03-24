using Backend.Domain.Facts;
using Backend.Enums;
using Backend.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Program.Tests.MockRepositories
{
    internal class MockFactStatusRepository : IFactStatusRepository
    {
        public ConcurrentBag<FactStatus> FactStatusRepository { get; } = new ConcurrentBag<FactStatus>();

        public Task<IEnumerable<FactStatus>> GetAllFactStatusAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(FactStatusRepository.AsEnumerable());
        }

        public Task<FactStatus> GetByTypeEntityIdFactAsync(FactEntityType entityType, string entityId, Fact fact, CancellationToken cancellationToken = default)
        {
            var factStatus = FactStatusRepository.SingleOrDefault(x => x.EntityType == entityType && x.EntityId == entityId && x.FactId == fact.Id);
            if (factStatus == null)
            {
                factStatus = new FactStatus()
                {
                    Id = Guid.NewGuid().ToString(),
                    EntityType = entityType,
                    EntityId = entityId,
                    Fact = fact,
                    FactId = fact.Id,
                    Name = fact.Name,
                    Status = false
                };
                FactStatusRepository.Add(factStatus);
            }
            return Task.FromResult(factStatus);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
