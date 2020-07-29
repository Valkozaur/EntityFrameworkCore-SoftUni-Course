using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using PetStore.Data.Common;

namespace PetStore.Data.Models
{
    public class MerchandiseType
    {
        public MerchandiseType()
        {
            this.Merchandises = new HashSet<Merchandise>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(GlobalConstants.MinMerchandiseTypeNameLength)]
        public string  Name { get; set; }

        public virtual ICollection<Merchandise> Merchandises { get; set; }
    }
}