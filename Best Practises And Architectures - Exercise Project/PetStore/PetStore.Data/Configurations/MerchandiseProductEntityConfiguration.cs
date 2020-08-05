using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Models;

namespace PetStore.Data.Configurations
{
    public class MerchandiseProductEntityConfiguration : IEntityTypeConfiguration<MerchandiseProduct>
    {
        public void Configure(EntityTypeBuilder<MerchandiseProduct> builder)
        {
            builder
                .HasKey(mp => new { mp.MerchandiseId, mp.ProductId });
        }
    }
}