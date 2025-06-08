using Microsoft.EntityFrameworkCore;
using CustomerApi.Data.Entities;

namespace CustomerApi.Data;

public class CustomerServiceDBContext(DbContextOptions<CustomerServiceDBContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Customer>())
        {
            if (entry.Entity is Auditable == false)
            {
                continue;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
