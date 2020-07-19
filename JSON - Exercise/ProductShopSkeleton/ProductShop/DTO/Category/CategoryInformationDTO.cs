using Newtonsoft.Json;

namespace ProductShop.DTO.Category
{
    public class CategoryInformationDTO
    {
        [JsonProperty("category")]
        public string CategoryName { get; set; }

        [JsonProperty("productCount")]
        public int ProductsCount { get; set; }

        [JsonProperty("averagePrice")]
        public string AveragePrice { get; set; }

        [JsonProperty("totalRevenue")]
        public string TotalRevenue { get; set; }
    }
}
