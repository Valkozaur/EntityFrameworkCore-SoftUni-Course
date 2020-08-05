using System;
using Cinema.Data.Models.Enums;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Movie
    {
        public Movie()
        {
            this.Projections = new HashSet<Projection>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)] //TODO: MIN LENGTH
        public string Title { get; set; }

        [Required]
        [Range(0, 9)]
        public Genre Genre { get; set; }

        public TimeSpan Duration { get; set; }

        [Required]
        [Range(1.0, 10.0)]
        public double Rating { get; set; }

        [Required]
        [MaxLength(20)]
        public string Director { get; set; }

        public ICollection<Projection> Projections { get; set; }
    }
}
