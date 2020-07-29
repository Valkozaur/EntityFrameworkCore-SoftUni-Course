using ProductShop.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType("Category")]
    public class ImportCategoryDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
