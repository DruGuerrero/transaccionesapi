using Ardalis.Specification.EntityFrameworkCore;
using Transacciones.Core.Interfaces;
using Transacciones.Infrastructure.Persistence;

namespace Transacciones.Infrastructure.Repositories;

public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public Repository(ApplicationDbContext context) : base(context) { }
}

public class ReadRepository<T> : RepositoryBase<T>, IReadRepository<T> where T : class
{
    public ReadRepository(ApplicationDbContext context) : base(context) { }
}
