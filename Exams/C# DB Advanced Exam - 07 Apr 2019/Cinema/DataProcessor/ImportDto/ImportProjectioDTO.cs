using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Cinema.Data.Models;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType(nameof(Projection))]
    public class ImportProjectioDTO
    {
        [Required]
        [XmlElement("MovieId")]
        public int MovieId { get; set; }

        [Required]
        [XmlElement("HallId")]
        public int HallId { get; set; }

        [Required]
        [XmlElement("DateTime")]
        public string DateTime { get; set; }
    }
}
