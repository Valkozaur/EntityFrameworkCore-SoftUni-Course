using Newtonsoft.Json;

namespace ProductShop.DTO.Users
{
    public class UserCountCollectionDTO
    {
        [JsonProperty("usersCount")]
        public int UsersCount { get; set; }
        
        [JsonProperty("users")]
        public UserSoldProductsDTO[] Users { get; set; }
    }
}
