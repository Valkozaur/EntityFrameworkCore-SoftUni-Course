using System;
using System.Collections.Generic;
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

        public int AvailableQuantity { get; set; }

        public string Notes { get; set; }
    }
}
