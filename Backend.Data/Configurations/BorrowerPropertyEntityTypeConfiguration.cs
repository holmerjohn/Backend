using Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    internal class BorrowerPropertyEntityTypeConfiguration : IEntityTypeConfiguration<BorrowerProperty>
    {
        public void Configure(EntityTypeBuilder<BorrowerProperty> builder)
        {
            builder.ToTable(nameof(BorrowerProperty));

            builder.Property(e => e.Id)
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

            builder.HasOne(property => property.Borrower)
                .WithMany(borrower => borrower.BorrowerProperties)
                .HasForeignKey(property => property.BorrowerId);

            builder.HasKey(e => e.Id);

        }
    }
}
