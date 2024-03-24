using Backend.Domain.Facts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    internal class FactStatusEntityTypeConfiguration : IEntityTypeConfiguration<FactStatus>
    {
        public void Configure(EntityTypeBuilder<FactStatus> builder)
        {
            builder.ToTable(nameof(FactStatus));

            builder.Property(e => e.Id)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.FactId)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.EntityType)
                .HasConversion<string>();
            
            builder.Property(e => e.Name)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Status);

            builder.HasOne(factStatus => factStatus.Fact)
                .WithMany()
                .HasForeignKey(factStatus => factStatus.FactId);

            builder.HasKey(e => e.Id);
        }
    }
}
