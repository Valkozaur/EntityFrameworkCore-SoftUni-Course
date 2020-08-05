using System.ComponentModel.DataAnnotations;
using PetStore.Data.Common;
using PetStore.Services.Mapping;

namespace PetStore.Services.Models.MerchandiseType.Input
{
    public class MerchandiseTypeInputServiceModel : IMapTo<Data.Models.MerchandiseType>
    {
        [Required]
        [MinLength(GlobalConstants.MinMerchandiseTypeNameLength)]
        [MaxLength(GlobalConstants.MaxMerchandiseTypeNameLength)]
        public string Name { get; set; }
    }
}
