using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Common;
using PetStore.Data.Models;

namespace PetStore.Data.Configurations
{
    public class BreedEntityConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder
                .Property(b => b.Name)
                .HasMaxLength(GlobalConstants.MaxBreedNameLength)
                .IsUnicode(true);
        }
    }
}