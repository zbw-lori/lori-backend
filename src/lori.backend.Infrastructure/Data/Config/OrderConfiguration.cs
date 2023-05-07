using lori.backend.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace lori.backend.Infrastructure.Data.Config;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.Property(o => o.ReceiptType)
      .HasConversion(o => o.ToString(), o => Enum.Parse<ReceiptType>(o));
  }
}
