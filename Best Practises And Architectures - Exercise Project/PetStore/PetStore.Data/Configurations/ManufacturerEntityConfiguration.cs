using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Common;
using PetStore.Data.Models;

namespace PetStore.Data.Configurations
{
    public class ManufacturerEntityConfiguration : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder
                .Property(m => m.Name)
                .HasMaxLength(GlobalConstants.MaxManufacturerNameLength)
                .IsUnicode(true);
        }
    }
}