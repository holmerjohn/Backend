using Backend.Domain.Facts;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories
{
    internal class FactRepository : EntityRepository<Fact>, IFactRepository
    {
        private readonly BackendDbContext _dbContext;

        public FactRepository(BackendDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get a Fact by Name
        /// </summary>
        /// <param name="factName"></param>
        /// <returns></returns>
        public Fact? GetByName(string? factName)
        {
            return _dbContext.Set<Fact>().SingleOrDefault(x => x.Name == factName);
        }

        /// <summary>
        /// Get a Fact by Name
        /// </summary>
        /// <remarks>This will create a new fact object if one is not found for that Name.  As such
        /// it should never return a null value.</remarks>
        /// <param name="factIdentifier"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Fact?> GetByNameAsync(string? factName, CancellationToken cancellationToken = default)
        {
            var fact = await _dbContext.Set<Fact>().SingleOrDefaultAsync(x => x.Name == factName);
            if (fact == null)
            {
                fact = Fact.CreateFact(factName);
                await _dbContext.Set<Fact>().AddAsync(fact);
            }
            return fact;
        }

        /// <summary>
        /// Get all the Fact definitions from the Database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Fact>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var facts = await _dbContext.Set<Fact>()
                .Include(f => f.Conditions)
                .ToListAsync(cancellationToken);

            return facts;

        }
    }
}
