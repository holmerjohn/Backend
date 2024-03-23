using Backend.Domain.Facts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    internal class FactConditionEntityTypeConfiguration : IEntityTypeConfiguration<FactCondition>
    {
        public void Configure(EntityTypeBuilder<FactCondition> builder)
        {
            builder.ToTable(nameof(FactCondition));

            builder.Property(e => e.Id)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Field)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Comparator)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(e => e.PropertyType)
                .IsRequired(false)
                .HasConversion<string>();

            builder.Property(e => e.Value)
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.HasKey(e => e.Id);
        }
    }
}
