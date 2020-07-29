using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Common;
using PetStore.Data.Models;

namespace PetStore.Data.Configurations
{
    public class MerchandiseTypeEntityConfiguration : IEntityTypeConfiguration<MerchandiseType>
    {
        public void Configure(EntityTypeBuilder<MerchandiseType> builder)
        {
            builder
                .Property(mt => mt.Name)
                .HasMaxLength(GlobalConstants.MaxMerchandiseTypeNameLength)
                .IsUnicode(true);
        }
    }
}