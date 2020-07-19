using Newtonsoft.Json;

namespace ProductShop.DTO.Product
{
    public class ProductCollectionDTO
    {
        [JsonProperty("count")]
        public int  Count { get; set; }

        [JsonProperty("products")]
        public ProductNamePriceDTO[] Products { get; set; }
    }
}
