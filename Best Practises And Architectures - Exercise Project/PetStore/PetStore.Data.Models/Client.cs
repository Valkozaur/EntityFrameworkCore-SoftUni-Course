using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using PetStore.Data.Common;

namespace PetStore.Data.Models
{
    public class Client
    {
        public Client()
        {
            this.Id = Guid.NewGuid().ToString();

            this.PurchasedOrders = new HashSet<Order>();
        }

        [Key]
        public string  Id { get; set; }

        [Required]
        [MinLength(GlobalConstants.MinUsernameLength)]
        public string Username { get; set; }

        [Required]
        [MinLength(GlobalConstants.MinPasswordLength)]
        public string Password { get; set; }

        [Required]
        [MinLength(GlobalConstants.MinEmailLength)]
        public string Email { get; set; }

        [Required]
        [MinLength(GlobalConstants.MinNameLength)]
        public string FirstName { get; set; }

        [MinLength(GlobalConstants.MinNameLength)]
        public string LastName { get; set; }

        public virtual ICollection<Order> PurchasedOrders { get; set; }
    }
}