using PetStore.Services.Models.Pet.InputModels;
using PetStore.Services.Models.Pet.OutputModels;

namespace PetStore.Services
{
    public interface IPetService
    {
        void AddPet(PetInputServiceModel model);

        PetDetailsViewModel FindPetById(int id);
    }
}
