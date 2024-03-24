using Backend.Domain.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    internal class LoanEntityTypeConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable(nameof(Loan));

            builder.Property(e => e.Id)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnicode(false);

            builder.HasKey(e => e.Id);
        }
    }
}
