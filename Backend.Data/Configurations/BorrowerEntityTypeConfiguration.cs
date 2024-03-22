using Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    internal class BorrowerEntityTypeConfiguration : IEntityTypeConfiguration<Borrower>
    {
        public void Configure(EntityTypeBuilder<Borrower> builder)
        {
            builder.ToTable(nameof(Borrower));

            builder.Property(e => e.Id)
                .IsRequired()
                .IsUnicode(false);

            builder.HasKey(e => e.Id);

            builder.HasMany(borrower => borrower.Loans)
                .WithMany(loan => loan.Borrowers);

        }
    }
}
