using System.Linq;

using AutoMapper;

using PetStore.Data;
using PetStore.Data.Models;

using PetStore.Services.Models.Pet.InputModels;
using PetStore.Services.Models.Pet.OutputModels;

namespace PetStore.Services
{
    public class PetService : IPetService
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
            var breed = this.context.Breeds.FirstOrDefault(x => x.Name == model.Breed);

            if (breed == null)
            {
                breed = new Breed() { Name = model.Breed };
            }

            bool? isMale = null;

            if (model.Gender == "Male")
            {
                isMale = true;
            }
            else if (model.Gender == "Female")
            {
                isMale = false;
            }

            var pet = new Pet()
            {
                Name = model.Name,
                Age = model.Age,
                Breed = breed,
                IsMale = isMale,
            };

            this.context.Pets.Add(pet);
            this.context.SaveChanges();
        }

        public bool RemovePet(string petId)
        {
            var pet = context.Pets.Find(petId);

            if (pet == null)
            {
                return false;
            }

            context.Pets.Remove(pet);
            context.SaveChanges();

            return true;

        }

        public PetDetailsViewModel FindPetById(int id)
        {
            var pet = context.Pets.FirstOrDefault(p => p.Id == id);

            if (pet == null)
            {
                return null;
            }

            var gender = "Unkown";

            if (pet.IsMale == true)
            {
                gender = "Male";
            }
            else if (pet.IsMale == false)
            {
                gender = "Female";
            }

            var petViewModel = new PetDetailsViewModel()
            {
                Name = pet.Name,
                Age = pet.Age,
                Breed = pet.Breed.Name,
                Gender = gender
            };

            return petViewModel;
        }
    }
}