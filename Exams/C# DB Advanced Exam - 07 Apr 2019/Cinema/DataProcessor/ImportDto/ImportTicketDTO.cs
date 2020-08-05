using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Cinema.Data.Models;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType(nameof(Ticket))]
    public class ImportTicketDTO
    {
        [Required]
        [XmlElement("ProjectionId")]
        public int ProjectionId { get; set; }

        [Required]
        [Range(0.01, Double.MaxValue)]
        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
