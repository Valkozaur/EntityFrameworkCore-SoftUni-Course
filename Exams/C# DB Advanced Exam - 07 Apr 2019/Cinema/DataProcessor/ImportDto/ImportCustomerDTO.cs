using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using System.Xml.Serialization;

using Cinema.Data.Models;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType(nameof(Customer))]
    public class ImportCustomerDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [XmlElement("LastName")]
        public string LastName { get; set; }

        [Range(12,110)]
        [XmlElement("Age")]
        public int Age { get; set; }

        [Range(0.01, Double.MaxValue)]
        [XmlElement("Balance")]
        public decimal Balance { get; set; }

        [XmlArray("Tickets")]
        public List<ImportTicketDTO> Tickets { get; set; }
    }
}
