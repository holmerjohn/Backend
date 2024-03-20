using Backend.Models;
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
                .IsRequired();

            builder.Property(e => e.BirthYear);


            builder.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Address)
                .HasMaxLength(1000)
                .IsUnicode(false);

            builder.HasKey(x => x.Id);

            builder.HasOne(borrower => borrower.Loan)
                .WithMany(loan => loan.Borrowers)
                .HasForeignKey(borrower => borrower.LoanId);

        }
    }
}
