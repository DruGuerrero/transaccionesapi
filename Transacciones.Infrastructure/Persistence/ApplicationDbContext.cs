using Microsoft.EntityFrameworkCore;
using Transacciones.Core.Entities.Account;
using Transacciones.Core.Entities.Transaction;

namespace Transacciones.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Accounts> Accounts => Set<Accounts>();
    public DbSet<Transactions> Transactions => Set<Transactions>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
