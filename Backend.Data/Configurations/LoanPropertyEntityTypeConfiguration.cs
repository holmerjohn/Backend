using Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    internal class LoanPropertyEntityTypeConfiguration : IEntityTypeConfiguration<LoanProperty>
    {
        public void Configure(EntityTypeBuilder<LoanProperty> builder)
        {
            builder.ToTable(nameof(LoanProperty));

            builder.Property(e => e.Id)
                .IsRequired();

            builder.Property(e => e.LoanId)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.ValueAsString)
                .IsRequired(false)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.HasKey(e => e.Id);

            builder.HasOne(property => property.Loan)
                .WithMany(loan => loan.LoanProperties)
                .HasForeignKey(property => property.LoanId);
        }
    }
}
