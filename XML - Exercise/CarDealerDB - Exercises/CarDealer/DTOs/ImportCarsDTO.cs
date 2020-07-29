using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("Car")]
    public class ImportCarsDTO
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("TraveledDistance")]
        public int TraveledDistance { get; set; }

        [XmlArray("parts")]
        public ImportPartCarDTO[] Parts { get; set; }
    }

    [XmlType("partId")]
    public class ImportPartCarDTO
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
