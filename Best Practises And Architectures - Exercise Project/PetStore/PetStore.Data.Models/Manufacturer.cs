using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using PetStore.Data.Common;

namespace PetStore.Data.Models
{
    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Products = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }
        
        [Required]
        [MinLength(GlobalConstants.MinManufacturerName)]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}