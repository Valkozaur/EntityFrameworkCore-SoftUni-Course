using Newtonsoft.Json;
using ProductShop.DTO.Product;

namespace ProductShop.DTO.Users
{
    public class UserSoldProductsDTO
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("soldProducts")]
        public ProductCollectionDTO SoldProducts { get; set; }
    }
}
