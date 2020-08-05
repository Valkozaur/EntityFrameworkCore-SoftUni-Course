using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
            this.Songs = new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }
        
        public DateTime ReleaseDate { get; set; }

        [NotMapped]
        public decimal Price { get; set; }

        [ForeignKey(nameof(Producer))]
        public int ProducerId { get; set; }
        public virtual Producer  Producer { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
