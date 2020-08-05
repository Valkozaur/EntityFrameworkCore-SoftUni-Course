using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [ForeignKey(nameof(Models.Customer))]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        [ForeignKey(nameof(Models.Projection))]
        public int ProjectionId { get; set; }

        public Projection Projection { get; set; }
    }
}