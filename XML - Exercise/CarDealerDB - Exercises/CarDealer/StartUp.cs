using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;
using CarDealer.XMLHelper;
using System;
using System.IO;
using System.Linq;

namespace CarDealer
{
    public class StartUp
    {
        private static string datasetsPath = "../../../Datasets/";
        private static string destinationPath = "../../../Datasets/ResultsXML/";

        private static string rootAttribute;
        private static string ImportOutputMessageFormat = "Successfully imported {0}";

        public static void Main()
        {
            var context = new CarDealerContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //var suppliersXML = File.ReadAllText(datasetsPath + "suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, suppliersXML));
            //var partsXML = File.ReadAllText(datasetsPath + "parts.xml");
            //Console.WriteLine(ImportParts(context, partsXML));
            //var carsXML = File.ReadAllText(datasetsPath + "cars.xml");
            //Console.WriteLine(ImportCars(context, carsXML));
            //var customersXML = File.ReadAllText(datasetsPath + "customers.xml");
            //Console.WriteLine(ImportCustomers(context, customersXML));
            //var salesXML = File.ReadAllText(datasetsPath + "sales.xml");
            //Console.WriteLine(ImportSales(context, salesXML));

            var result = GetSalesWithAppliedDiscount(context);
            File.WriteAllText(destinationPath + "sales-discounts.xml", result);

            Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            rootAttribute = "Suppliers";

            var suppliersDTOs = XMLConverter.Deserializer<ImportSuppliersDTO>(inputXml, rootAttribute);

            var suppliers = suppliersDTOs
                .Select(s => new Supplier
                {
                    Name = s.Name,
                    IsImporter = s.IsImporter
                })
                .ToArray();

            context.AddRange(suppliers);
            context.SaveChanges();

            return string.Format(ImportOutputMessageFormat, suppliers.Length);
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            rootAttribute = "Parts";

            var partDTOs = XMLConverter.Deserializer<ImportPartsDTO>(inputXml, rootAttribute);

            var parts = partDTOs
                .Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId))
                .Select(p => new Part
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierId = p.SupplierId,
                })
                .ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return string.Format(ImportOutputMessageFormat, parts.Length);
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            rootAttribute = "Customers";

            var customerDTOs = XMLConverter.Deserializer<ImportCustomerDTO>(inputXml, rootAttribute);

            var customers = customerDTOs
                .Select(c => new Customer
                {
                    Name = c.Name,
                    BirthDate = DateTime.Parse(c.BirthDate),
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return string.Format(ImportOutputMessageFormat, customers.Length);
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            rootAttribute = "Cars";

            var carsDTOs = XMLConverter.Deserializer<ImportCarsDTO>(inputXml, rootAttribute);

            var cars = carsDTOs
                .Select(c => new Car
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TraveledDistance,
                    PartCars = c.Parts
                        .Where(p => context.Parts.Any(x => x.Id == p.PartId))
                        .Select(p => p.PartId)
                        .Distinct()
                        .Select(Id => new PartCar
                        {
                            PartId = Id
                        })
                        .ToArray()
                })
                .ToArray();

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return string.Format(ImportOutputMessageFormat, cars.Length);
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            rootAttribute = "Sales";

            var salesDTOs = XMLConverter.Deserializer<ImportSalesDTO>(inputXml, rootAttribute);

            var sales = salesDTOs
                .Where(s => context.Cars.Any(c => c.Id == s.CarId))
                .Select(c => new Sale
                {
                    CustomerId = c.CustomerId,
                    CarId = c.CarId,
                    Discount = c.Discount,
                })
                .ToArray();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return string.Format(ImportOutputMessageFormat, sales.Length);
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            rootAttribute = "cars";

            var carDTOs = context.Cars
                .Where(c => c.TravelledDistance >= 2_000_000)
                .Select(c => new ExportCarsWithDistance
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            var xmlResult = XMLConverter.Serialize<ExportCarsWithDistance>(carDTOs, rootAttribute);

            return xmlResult;
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            rootAttribute = "cars";

            var carsDTOs = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new ExportCarsFromBMWDTO
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            var resultXML = XMLConverter.Serialize<ExportCarsFromBMWDTO>(carsDTOs, rootAttribute);

            return resultXML;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            rootAttribute = "suppliers";

            var localSuppliersDTOs = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(s => new ExportLocalSupplierDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count,
                })
                .ToArray();

            var resultXML = XMLConverter.Serialize<ExportLocalSupplierDTO>(localSuppliersDTOs, rootAttribute);

            return resultXML;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            rootAttribute = "cars";

            var carsWithPartsDTOs = context.Cars
                .Select(c => new ExportCarsWithPartsDTO
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars
                        .Select(ps => new PartsNamePriceDTO
                        {
                            Name = ps.Part.Name,
                            Price = ps.Part.Price,
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            var resultXML = XMLConverter.Serialize<ExportCarsWithPartsDTO>(carsWithPartsDTOs, rootAttribute);

            return resultXML;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            rootAttribute = "customers";

            var customersWithSales = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new ExportCustomerSalesDTO
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.SelectMany(x => x.Car.PartCars).Sum(pc => pc.Part.Price)
                })
                .OrderByDescending(x => x.SpentMoney)
                .ToArray();

            var resultXML = XMLConverter.Serialize<ExportCustomerSalesDTO>(customersWithSales, rootAttribute);

            return resultXML;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            rootAttribute = "sales";

            var salesInfoDTOs = context.Sales
                .Select(s => new ExportSalesWithDiscountDTO
                {
                    Car = new CarInfoDTO
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(ps => ps.Part.Price),
                    PriceWithDiscount = s.Car.PartCars.Sum(ps => ps.Part.Price) - s.Car.PartCars.Sum(ps => ps.Part.Price) * s.Discount / 100
                })
                .ToArray();

            var resultXML = XMLConverter.Serialize<ExportSalesWithDiscountDTO>(salesInfoDTOs, rootAttribute);

            return resultXML;
        }
    }
}