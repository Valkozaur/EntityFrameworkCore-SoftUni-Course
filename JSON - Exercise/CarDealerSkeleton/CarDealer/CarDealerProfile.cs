using System.Globalization;
using System.Linq;
using AutoMapper;
using CarDealer.DTO.CarsDTOs;
using CarDealer.DTO.CustomersDTOs;
using CarDealer.DTO.PartsDTOs;
using CarDealer.DTO.SalesDTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Porblem 14
            this.CreateMap<Customer, CustomerNameBirthDateYoungDriverDTO>()
                .ForMember(x => x.BirthDate,
                y => y.MapFrom(x => x.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));

            //Problem 15
            this.CreateMap<Car, FullCarInforDTO>();

            //Problem 16
            this.CreateMap<Part, PartNamePriceDTO>()
                .ForMember(x => x.Price,
                y => y.MapFrom(x => x.Price.ToString("F2")));

            this.CreateMap<Car, CarMakeModelTravelLedDTO>();

            //Problem 17
            this.CreateMap<Customer, CustomersWithBoughtCarsCountDTO>()
                .ForMember(x => x.FullName,
                y => y.MapFrom(x => x.Name))
                .ForMember(x => x.BoughtCars,
                y => y.MapFrom(x => x.Sales.Count))
                .ForMember(x => x.SpentMoney,
                y => y.MapFrom(x => x.Sales
                                .SelectMany(s => s.Car.PartCars
                                                .Select(p => p.Part.Price)).Sum()));

            //Problem 18
            this.CreateMap<Sale, SaleInfoDTO>()
                .ForMember(x => x.Price, 
                y => y.MapFrom(x => $"{x.Car.PartCars.Sum(p => p.Part.Price):F2}"))
                .ForMember(x => x.PriceWithDiscount,
                 y => y.MapFrom(x => $"{x.Car.PartCars.Sum(p => p.Part.Price) - (1.00m * (x.Discount / 100)):F2}"));
        }
    }
}
