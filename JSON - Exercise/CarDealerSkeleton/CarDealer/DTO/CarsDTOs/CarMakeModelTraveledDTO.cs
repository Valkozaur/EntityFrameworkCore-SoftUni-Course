using CarDealer.DTO.PartsDTOs;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CarDealer.DTO.CarsDTOs
{
    public class CarMakeModelTravelLedDTO
    {
        public CarMakeModelTravelLedDTO()
        {
            this.Parts = new List<PartNamePriceDTO>();
        }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        [JsonPropertyName("parts"), JsonIgnore]
        public List<PartNamePriceDTO> Parts { get; set; }
    }
}
