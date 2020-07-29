using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PetStore.Data.Common;
using PetStore.Data.Models;

namespace PetStore.Data.Configurations
{
    public class ClientEntityConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder
                .Property(c => c.Username)
                .HasMaxLength(GlobalConstants.MaxUsernameLength)
                .IsUnicode(false);

               builder
                .Property(c => c.Password)
                .IsUnicode(false);

            builder
                .Property(c => c.Email)
                .HasMaxLength(GlobalConstants.MaxEmailLength)
                .IsUnicode(false);

            builder
                .Property(c => c.FirstName)
                .HasMaxLength(GlobalConstants.MaxNameLength)
                .IsUnicode(true);

            builder
                .Property(c => c.LastName)
                .HasMaxLength(GlobalConstants.MaxNameLength)
                .IsUnicode(true);
        }
    }
}