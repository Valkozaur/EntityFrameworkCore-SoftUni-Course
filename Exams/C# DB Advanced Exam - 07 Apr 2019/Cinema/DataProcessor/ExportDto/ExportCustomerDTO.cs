using System;
using System.Xml.Serialization;
using Cinema.Data.Models;

namespace Cinema.DataProcessor.ExportDto
{
    [XmlType(nameof(Customer))]
    public class ExportCustomerDTO
    {
        [XmlAttribute("FirstName")]
        public string FirstName { get; set; }

        [XmlAttribute("LastName")]
        public string LastName { get; set; }

        [XmlElement("SpentMoney")]
        public string SpentMoney { get; set; }

        [XmlElement("SpentTime")]
        public string SpentTime { get; set; }
    }
}
