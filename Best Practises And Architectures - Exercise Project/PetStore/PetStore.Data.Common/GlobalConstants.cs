namespace PetStore.Data.Common
{
    public static class GlobalConstants
    {
        //Global
        public const int MaxNotesLenght = 255;

        //Client
        public const int MinUsernameLength = 6;
        public const int MaxUsernameLength = 50;

        public const int MinPasswordLength = 8;

        public const int MinEmailLength = 6;
        public const int MaxEmailLength = 80;

        public const int MinNameLength = 3;
        public const int MaxNameLength = 80;

        //Merchandise
        public const double MerchandiseMinPrice = 0;

        //MerchandiseType
        public const int MinMerchandiseTypeNameLength = 3;
        public const int MaxMerchandiseTypeNameLength = 40;

        //Pet
        public const int MinPetNameLength = 3;
        public const int MaxPetNameLength = 100;

        public const int MinPetAge = 0;
        public const int MaxPetAge = 200;

        //Breed
        public const int MinBreedNameLength = 2;
        public const int MaxBreedNameLength = 50;

        //Product
        public const int MinProductNameLength = 3;
        public const int MaxProductNameLength = 80;

        //Manufacturer
        public const int MinManufacturerName = 5;
        public const int MaxManufacturerNameLength = 60;
    }
}
