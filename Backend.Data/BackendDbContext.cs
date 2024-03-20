using Backend.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    internal class BackendDbContext : DbContext
    {
        public BackendDbContext() 
        { 
        }
        
        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BorrowerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LoanEntityTypeConfiguration());


        }

    }
}
