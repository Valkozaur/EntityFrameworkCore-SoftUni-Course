using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("car")]
    public class ExportCarsWithPartsDTO
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public PartsNamePriceDTO[] Parts { get; set; }
    }

    [XmlType("part")]
    public class PartsNamePriceDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
