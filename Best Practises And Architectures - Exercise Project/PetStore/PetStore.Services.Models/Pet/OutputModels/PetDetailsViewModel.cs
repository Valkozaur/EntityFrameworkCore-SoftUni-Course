using AutoMapper;

using PetStore.Services.Mapping;

namespace PetStore.Services.Models.Pet.OutputModels
{
    public class PetDetailsViewModel
    {
        public string Name { get; set; }

        public string Breed { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }
    }
}
