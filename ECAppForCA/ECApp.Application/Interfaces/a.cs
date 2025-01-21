using ECApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ECApp.Application.Interfaces;

public interface IECDbContext
{
    Task<Guid> NewId();
    public DbSet<SchemaVersion> SchemaVersions { get; set; }
    public DbSet<Users> Users { get; set; }
    public DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}