using System.Xml.Serialization;
using ProductShop.Dtos.Export.Product;

namespace ProductShop.Dtos.Export.Users
{
    public class UsersCountDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public UserFullInfoDTO[] Users { get; set; }
    }


    [XmlType("User")]
    public class UserFullInfoDTO
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age{ get; set; }

        [XmlElement("SoldProducts")]
        public SoldProductsDTO SoldProducts { get; set; }
    }

    [XmlType("SoldProducts")]
    public class SoldProductsDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public ProductNameAndPriceDTO[] Products { get; set; }
    }
}
