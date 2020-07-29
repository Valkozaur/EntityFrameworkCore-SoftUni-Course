using ProductShop.Dtos.Export.Product.InformationInterfaces;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export.Product
{
    [XmlType("Product")]
    public class ProductNameAndPriceDTO : INameDetails, IPriceDetails
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
