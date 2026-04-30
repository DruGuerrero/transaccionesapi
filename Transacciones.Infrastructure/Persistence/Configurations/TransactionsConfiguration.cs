using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transacciones.Core.Entities.Transaction;

namespace Transacciones.Infrastructure.Persistence.Configurations
{
    public class TransactionsConfiguration : IEntityTypeConfiguration<Transactions>
    {
        public void Configure(EntityTypeBuilder<Transactions> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Account)
                   .WithMany(a => a.Transactions)
                   .HasForeignKey(t => t.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
