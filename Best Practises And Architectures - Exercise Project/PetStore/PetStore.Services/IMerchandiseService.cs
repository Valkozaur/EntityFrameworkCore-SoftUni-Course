using PetStore.Services.Models.MerchandiseType.Input;

namespace PetStore.Services
{
    public interface IMerchandiseService
    {
        void AddMerchandise(MerchandiseTypeInputServiceModel merchandiseType, int id);

        void SellMerchandise(int merchandiseId);

        
    }
}
