using System.Reflection;
using lori.backend.Core.Models;
using lori.backend.SharedKernel;
using lori.backend.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lori.backend.Infrastructure.Data;

public class LoriDbContext : DbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;

  public LoriDbContext(DbContextOptions<LoriDbContext> options,
    IDomainEventDispatcher? dispatcher)
      : base(options)
  {
    _dispatcher = dispatcher;
  }

  public DbSet<Address> Addresses { get; set; } = null!;
  public DbSet<Customer> Customers { get; set; } = null!;
  public DbSet<Item> Items { get; set; } = null!;
  public DbSet<Role> Roles { get; set; } = null!;
  public DbSet<Login> Logins { get; set; } = null!;
  public DbSet<Order> Orders { get; set; } = null!;

  public DbSet<OrderItem> OrderItems { get; set; } = null!;
  public DbSet<Robot> Robots { get; set; } = null!;
  public DbSet<Role> Roles { get; set; } = null!;
  public DbSet<Route> Routes { get; set; } = null!;
  public DbSet<Store> Stores { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
        .Select(e => e.Entity)
        .Where(e => e.DomainEvents.Any())
        .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }
}
