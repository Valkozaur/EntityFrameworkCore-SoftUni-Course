using System;

using System.ComponentModel.DataAnnotations;

using Cinema.Data.Models.Enums;

namespace Cinema.DataProcessor.ImportDto
{
    public class ImportMoviesDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Title { get; set; }

        public string Genre { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Range(1, 10)]
        public double Rating { get; set; }
        
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Director { get; set; }
    }
}
