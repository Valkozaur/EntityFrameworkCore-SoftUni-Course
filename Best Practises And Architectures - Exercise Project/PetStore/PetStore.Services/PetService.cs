using AutoMapper;
using PetStore.Data;
using PetStore.Services.Models.Pet.InputModels;
using PetStore.Services.Models.Pet.OutputModels;

namespace PetStore.Services
{
    public  class PetService : IPetService
    {
        private PetStoreDbContext context;
        private IMapper mapper;

        public PetService(PetStoreDbContext context, IMapper mapper)
        {
            this.context = context;

            this.mapper = mapper;
        }

        public void AddPet(PetInputServiceModel model)
        {
            
        }

        public PetDetailsViewModel FindPetById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}