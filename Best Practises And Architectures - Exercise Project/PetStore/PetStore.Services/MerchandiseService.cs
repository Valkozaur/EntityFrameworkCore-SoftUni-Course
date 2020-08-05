using System;
using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Models.MerchandiseType.Input;
using Services.GlobalMessages;

namespace PetStore.Services
{
    public class MerchandiseService : IMerchandiseService
    {
        private PetStoreDbContext context;

        public MerchandiseService(PetStoreDbContext context)
        {
            this.context = context;
        }

        public void AddMerchandise(MerchandiseTypeInputServiceModel merchandiseType, int id)
        {
            var merchandise = new Merchandise();

            if (merchandiseType.Name == "Pet")
            {
                var pet = context.Pets.Find(id);

                if (pet == null)
                {
                    throw new ArgumentException(ErrorMessages.PetNotFound);
                }

                
            

        }
            else
            {

            }
        }

        public void SellMerchandise(int merchandiseId)
        {
            throw new System.NotImplementedException();
        }
    }
}
