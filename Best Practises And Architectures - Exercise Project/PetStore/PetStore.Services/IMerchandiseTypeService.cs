namespace PetStore.Services
{
    public interface IMerchandiseTypeService
    {
        void CreateMerchandiseType(string name);

        bool RemoveMerchandiseType(string name);

        bool RemoveMerchandiseType(int id);
    }
}
