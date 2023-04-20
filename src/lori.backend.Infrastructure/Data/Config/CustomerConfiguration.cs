using lori.backend.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace lori.backend.Infrastructure.Data.Config;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> builder)
  {
    builder.HasOne(c => c.Address)
      .WithMany()
      .HasForeignKey(c => c.AddressId);

    builder.HasOne(c => c.Login)
      .WithOne()
      .HasForeignKey<Customer>(c => c.Username);
  }
}
