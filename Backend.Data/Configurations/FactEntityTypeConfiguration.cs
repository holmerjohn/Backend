using Backend.Domain.Facts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    internal class FactEntityTypeConfiguration : IEntityTypeConfiguration<Fact>
    {
        public void Configure(EntityTypeBuilder<Fact> builder)
        {
            builder.ToTable(nameof(Fact));

            builder.Property(e => e.Id)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Name)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.EntityType)
                .IsRequired()
                .HasConversion<string>();

            builder.HasKey(e => e.Id);

            builder.HasMany(fact => fact.Conditions)
                .WithOne(factCondition => factCondition.Fact)
                .HasForeignKey(factCondition => factCondition.FactId);
        }
    }


}
