using lori.backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace lori.backend.Infrastructure.Data.Config;
public class LoginConfiguration : IEntityTypeConfiguration<Login>
{
  public void Configure(EntityTypeBuilder<Login> builder)
  {
    builder.HasOne(l => l.Role)
      .WithMany()
      .HasForeignKey(l => l.RoleId);
  }
}
