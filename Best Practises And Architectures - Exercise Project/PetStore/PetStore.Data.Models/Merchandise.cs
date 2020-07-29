using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using PetStore.Data.Common;

namespace PetStore.Data.Models
{
    public class Merchandise
    {
        public Merchandise()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Range(GlobalConstants.MerchandiseMinPrice, double.MaxValue)]
        public decimal Price { get; set; }

        [ForeignKey(nameof(MerchandiseType))]
        public int MerchandiseTypeId { get; set; }
        public virtual MerchandiseType MerchandiseType { get; set; }

        [ForeignKey(nameof(Pet))]
        public int? PetId { get; set; }
        public virtual Pet Pet { get; set; }

        [ForeignKey(nameof(Product))]
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }

        public bool IsBought { get; set; }

        public string Notes { get; set; }
    }
}
