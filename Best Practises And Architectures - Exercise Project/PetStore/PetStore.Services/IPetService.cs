using PetStore.Services.Models.Pet.InputModels;
using PetStore.Services.Models.Pet.OutputModels;

namespace PetStore.Services
{
    public interface IPetService
    {
        void AddPet(PetInputServiceModel model);

        bool RemovePet(string petId);

        PetDetailsViewModel FindPetById(int id);
    }
}
