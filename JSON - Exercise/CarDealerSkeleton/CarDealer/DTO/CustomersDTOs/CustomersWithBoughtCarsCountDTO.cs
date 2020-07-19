using System.Text.Json.Serialization;

namespace CarDealer.DTO.CustomersDTOs
{
    public class CustomersWithBoughtCarsCountDTO
    {
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("boughtCars")]
        public int BoughtCars { get; set; }

        [JsonPropertyName("spentMoney")]
        public decimal SpentMoney { get; set; }
    }
}
