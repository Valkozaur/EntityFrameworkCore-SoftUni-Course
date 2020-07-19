using Newtonsoft.Json;
using ProductShop.DTO.Product;

namespace ProductShop.DTO.Users
{
    public class UsersSoldItemsDTO 
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName{ get; set; }

        [JsonProperty("soldProducts")]
        public ProductDTO[] SoldProducts { get; set; }
    }
}
