using Backend.Domain.Facts;
using Backend.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Program.Tests.MockRepositories
{
    internal class MockFactRepository : IFactRepository
    {
        public ConcurrentBag<Fact> FactRepository { get; } = new ConcurrentBag<Fact>();

        public Task<IEnumerable<Fact>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(FactRepository.AsEnumerable());
        }

        public Fact? GetByName(string? name)
        {
            return FactRepository.SingleOrDefault(x => x.Name == name);
        }

        public Task<Fact?> GetByNameAsync(string? name, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(FactRepository.SingleOrDefault(x => x.Name == name));
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
