using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Models;

namespace PetStore.Data.Configurations
{
    public partial class MerchandisePetEntityConfiguration : IEntityTypeConfiguration<MerchandisePet>
    {
        public void Configure(EntityTypeBuilder<MerchandisePet> builder)
        {
            builder
                .HasKey(mp => new { mp.MerchandiseId, mp.PetId });
        }
    }
}