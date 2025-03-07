using ECApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECApp.Application.Common.Interfaces;

public interface IECDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    public DbSet<Users> Users { get; set; }
}