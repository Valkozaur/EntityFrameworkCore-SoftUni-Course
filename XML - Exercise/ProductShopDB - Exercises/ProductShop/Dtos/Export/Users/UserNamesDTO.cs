using System.Collections.Generic;
using System.Xml.Serialization;
using ProductShop.Dtos.Export.Product;
using ProductShop.Dtos.Export.Users.UserDetaildInterfaces;

namespace ProductShop.Dtos.Export.Users
{
    [XmlType("User")]
    public class UserProductsDTO : IFirstNameDetails, ILastNameDetails, ISoldProductDetails
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public ProductNameAndPriceDTO[] SoldProducts { get; set; }
    }
}
