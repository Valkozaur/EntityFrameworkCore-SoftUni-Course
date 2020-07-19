using Newtonsoft.Json;

namespace ProductShop.DTO.Product
{
    public class ProductNamePriceDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
