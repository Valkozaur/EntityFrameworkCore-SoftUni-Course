using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Common;
using PetStore.Data.Models;

namespace PetStore.Data.Configurations
{
    public class MerchandiseEntityConfiguration : IEntityTypeConfiguration<Merchandise>
    {
        public void Configure(EntityTypeBuilder<Merchandise> builder)
        {
            builder
                .Property(m => m.Notes)
                .HasMaxLength(GlobalConstants.MaxNotesLenght);
        }
    }
}
