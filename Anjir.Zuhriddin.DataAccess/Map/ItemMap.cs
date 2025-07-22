using Anjir.Zuhriddin.Items.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anjir.Zuhriddin.Items.DataAccess.Map
{
    internal class ItemMap : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item");

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(400)
                .IsUnicode(false);

            builder.Property(t => t.Type)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(t => t.Date)
                .IsRequired();
        }
    }
}
