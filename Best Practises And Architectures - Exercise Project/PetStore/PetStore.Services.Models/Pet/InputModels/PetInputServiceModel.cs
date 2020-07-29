using System.ComponentModel.DataAnnotations;

using PetStore.Data.Common;

namespace PetStore.Services.Models.Pet.InputModels
{
    public class PetInputServiceModel
    {
        [Required]
        [MinLength(GlobalConstants.MinPetNameLength)]
        [MaxLength(GlobalConstants.MaxPetNameLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(GlobalConstants.MinBreedNameLength)]
        [MaxLength(GlobalConstants.MaxBreedNameLength)]
        public string Breed { get; set; }

        [Range(GlobalConstants.MinPetAge, GlobalConstants.MaxPetAge)]
        public int Age { get; set; }

        public bool? IsMale { get; set; }

        public decimal Price { get; set; }
    }
}
