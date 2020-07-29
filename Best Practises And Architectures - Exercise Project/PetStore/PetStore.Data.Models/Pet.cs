using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using PetStore.Data.Common;

namespace PetStore.Data.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(GlobalConstants.MinPetNameLength)]
        public string Name { get; set; }

        [ForeignKey(nameof(Breed))]
        public int  BreedId { get; set; }
        public Breed Breed { get; set; }

        [Range(GlobalConstants.MinPetAge, GlobalConstants.MaxPetAge)]
        public int Age { get; set; }

        public bool? IsMale { get; set; }
    }
}