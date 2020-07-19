using CarDealer.DTO.CarsDTOs;
using System.Text.Json.Serialization;

namespace CarDealer.DTO.SalesDTO
{
    public class SaleInfoDTO
    {
        [JsonPropertyName("car")]
        public CarMakeModelTravelLedDTO Car { get; set; }

        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("discount")]
        public string Discount { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("priceWithDiscount")]
        public string PriceWithDiscount { get; set; }
    }
}
