using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Models;

namespace PetStore.Data.Configurations
{
    public class OrderMerchandiseEntityConfiguration : IEntityTypeConfiguration<OrderMerchandise>
    {
        public void Configure(EntityTypeBuilder<OrderMerchandise> builder)
        {
            builder
                .HasKey(m => new { m.OrderId, m.MerchandiseId });
        }
    }
}
