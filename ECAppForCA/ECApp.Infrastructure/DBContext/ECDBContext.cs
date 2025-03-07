using ECApp.Application.Common.Interfaces;
using ECApp.Domain.Entities;
using ECApp.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ECApp.Infrastructure.DBContext;

public partial class ECDBContext : DbContext, IECDbContext
{
    public ECDBContext(DbContextOptions<ECDBContext> options) : base(options)
    {
    }

    public Task<Guid> NewId()
    {
        return Task.FromResult(new IdHelper().GetId());
    }

    public DbSet<SchemaVersion> SchemaVersions { get; set; }
    public DbSet<Users> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SchemaVersion>(entity =>
        {
            entity.HasKey(e => new { e.Id });

            entity.ToTable("SchemaVersions");

            entity.Property(e => e.Id).HasComment("識別碼");
            entity.Property(e => e.CreatedTime).HasComment("建立時間");
            entity.Property(e => e.Creator).HasComment("建立者");
            entity.Property(e => e.Data).HasComment("資料容器");
            entity.Property(e => e.LatestUpdatedTime).HasComment("最新更新時間");
            entity.Property(e => e.LatestUpdater).HasComment("最後更新者");

            entity.Property(e => e.RawKey)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("物件類別鍵值");

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasComment("物件名稱");

            entity.Property(e => e.PartitionKey)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasComment("使用者名稱");
            entity.Property(e => e.CreatedTime).HasComment("建立時間");
            entity.Property(e => e.DeletedTime).HasComment("刪除時間");
            entity.Property(e => e.Creator).HasComment("建立者");
            entity.Property(e => e.LatestUpdatedTime).HasComment("最新更新時間");
            entity.Property(e => e.LatestUpdater).HasComment("最後更新者");
        });
    }
}