using Microsoft.EntityFrameworkCore;
using WEBAPI.Domain.Entities;

namespace WEBAPI.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Yeni kayıt eklerken CreatedAt otomatik set edilir, güncellemede ise createdAt değiştirilmesi engelleniyor..
        foreach (var entry in ChangeTracker.Entries<TodoItem>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // entity'nin configürasyonunu yapıyoruz veritabanı için.
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();
        });
    }
}
