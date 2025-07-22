using Anjir.Zuhriddin.Items.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anjir.Zuhriddin.Items.DataAccess.Map
{
    internal class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasAlternateKey(t => t.Email);

            builder.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(400)
                .IsUnicode(false);

            builder.Property(t => t.PasswordHash)
                .IsRequired()
                .HasMaxLength(400)
                .IsUnicode(false);

            builder.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(400)
                .IsUnicode(false);

            builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(400)
                .IsUnicode(false);
        }
    }
}
