using Backend.Models;
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
                .IsRequired();

            builder.Property(e => e.LoanAmount);
            builder.Property(e => e.PurchasePrice);


            builder.Property(e => e.LoanType)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.PropertyAddress)
                .HasMaxLength(1000)
                .IsUnicode(false);

            builder.HasKey(x => x.Id);

            
        }
    }
}
