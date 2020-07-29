using System;
using System.Linq;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Data.Models
{
    public class Order
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Merchandises = new HashSet<OrderMerchandise>();
        }

        [Key]
        public string Id { get; set; }
        
        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; }
        public virtual Client Client { get; set; }

        public virtual ICollection<OrderMerchandise> Merchandises { get; set; }

        [NotMapped]
        public decimal TotalPrice => Merchandises.Sum(x => x.Merchandise.Price);

        public string Notes { get; set; }
    }
}
