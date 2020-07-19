using System;
using System.IO;
using System.Linq;
using System.Text.Json;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CarDealer.Data;
using CarDealer.Models;
using CarDealer.DTO.CarsDTOs;
using CarDealer.DTO.SalesDTO;
using CarDealer.DTO.CustomersDTOs;

namespace CarDealer
{
    public class StartUp
    {
        private static JsonSerializerOptions options;
        private static string path = "../../../Datasets/JsonResults/";

        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            //EnsureDatabaseIsCreated(context);
            CreateJsonSettings();
            InitializeMapper();

            var jsonResult = GetSalesWithAppliedDiscount(context);

            Console.WriteLine(jsonResult);

            File.WriteAllText(path + "sales-discounts.json", jsonResult);
        }

        private static void EnsureDatabaseIsCreated(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            Console.WriteLine("Database was deleted!");
            context.Database.EnsureCreated();
            Console.WriteLine("Database was created!");
        }

        private static void CreateJsonSettings()
        {
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());
        }

        //Problem 8
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonSerializer.Deserialize<Supplier[]>(inputJson, options);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";
        }

        //Problem 9
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonSerializer.Deserialize<Part[]>(inputJson, options)
                .Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId))
                .ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Succesfully imported {parts.Count()}";
        }

        //Problem 10
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var cars = JsonSerializer.Deserialize<Car[]>(inputJson, options);

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Length}";
        }

        //Problem 11
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonSerializer.Deserialize<Customer[]>(inputJson, options)
                .ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }


        //Problem 12
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonSerializer.Deserialize<Sale[]>(inputJson, options)
                .ToArray();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}";
        }


        //Problem 13
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver == true)
                .ProjectTo<CustomerNameBirthDateYoungDriverDTO>()
                .ToList();

            var jsonResult = JsonSerializer.Serialize(customers, options);

            return jsonResult;
        }


        //Problem 14
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars
                .Where(c => c.Make == "Toyota")
                .ProjectTo<FullCarInforDTO>()
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            var jsonResult = JsonSerializer.Serialize(toyotaCars, options);

            return jsonResult;
        }

        //Problem 15
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithParts = context.Cars
                .ProjectTo<CarMakeModelTravelLedDTO>()
                .ToList();

            var jsonResult = JsonSerializer.Serialize(carsWithParts, options);

            return jsonResult;
        }

        //Problem 16
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var sales = context.Customers
                //.Select(c => new
                //{
                //    fullName = c.Name,
                //    boughtCars = c.Sales.Count,
                //    spentMoney = c.Sales
                //        .SelectMany(s => s.Car.PartCars.Select(p => p.Part.Price))
                //        .Sum()
                //})
                .ProjectTo<CustomersWithBoughtCarsCountDTO>()
                .ToList();

            var jsonResult = JsonSerializer.Serialize(sales, options);

            return jsonResult;
        }

        //Problem 17
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                //.Select(s => new
                //{
                //    car = new
                //    {
                //        Make = s.Car.Make,
                //        Model = s.Car.Model,
                //        TravelledDistance = s.Car.TravelledDistance
                //    },
                //    customerName = s.Customer.Name,
                //    Discount = s.Discount.ToString("F2"),
                //    price = s.Car.PartCars.Sum(pc => pc.Part.Price).ToString("F2"),
                //    priceWithDiscount = $"{s.Car.PartCars.Sum(y => y.Part.Price) - (1.00m * (s.Discount / 100)):F2}"
                //})
                .ProjectTo<SaleInfoDTO>()
                .Take(10)
                .ToList();

            var jsonResult = JsonSerializer.Serialize(sales, options);

            return jsonResult;
        }
    }
}