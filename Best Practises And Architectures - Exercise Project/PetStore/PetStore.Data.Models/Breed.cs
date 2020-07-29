using System.ComponentModel.DataAnnotations;

using PetStore.Data.Common;

namespace PetStore.Data.Models
{
    public class Breed
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(GlobalConstants.MinBreedNameLength)]
        public string Name { get; set; }
    }
}