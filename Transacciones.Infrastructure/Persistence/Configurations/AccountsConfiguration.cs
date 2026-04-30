using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transacciones.Core.Entities.Account;
namespace Transacciones.Infrastructure.Persistence.Configurations;

public class AccountsConfiguration : IEntityTypeConfiguration<Accounts>
{
    public void Configure(EntityTypeBuilder<Accounts> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.AccountNumber)
               .IsUnique();
    }
}