using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Common;
using PetStore.Data.Models;

namespace PetStore.Data.Configurations
{
    public class PetEntityConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder
                .Property(p => p.Name)
                .HasMaxLength(GlobalConstants.MaxPetNameLength)
                .IsUnicode(true);
        }
    }
}