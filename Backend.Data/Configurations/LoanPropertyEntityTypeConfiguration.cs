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

            builder.Property(e => e.PropertyType)
                .HasMaxLength(20)
                .HasConversion<string>();

            builder.Property(e => e.StringValue)
                .IsRequired(false)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.NumberValue)
                .IsRequired(false);


            builder.HasKey(e => e.Id);

            builder.HasOne(property => property.Loan)
                .WithMany(loan => loan.LoanProperties)
                .HasForeignKey(property => property.LoanId);
        }
    }
}
