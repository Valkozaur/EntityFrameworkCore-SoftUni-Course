using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.Dtos.Export.Category;
using ProductShop.Dtos.Export.Product;
using ProductShop.Dtos.Export.Users;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        private readonly static string destinationPath = "../../../Datasets/XmlResults/";

        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            var context = new ProductShopContext();
            var result = GetUsersWithProducts(context);

            Console.WriteLine(result);
        }


        private static void ResetDatabase(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            Console.WriteLine("Database was deleted successfully!");
            context.Database.EnsureCreated();
            Console.WriteLine("Database was created successfully!");
        }

        //Query 1

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportCategoryDTO[]), new XmlRootAttribute("Categories"));

            ImportCategoryDTO[] categoriesDTOs;

            using (var stream = new StringReader(inputXml))
            {
                categoriesDTOs = (ImportCategoryDTO[])xmlSerializer.Deserialize(stream);
            }

            var categories = Mapper.Map<Category[]>(categoriesDTOs);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully impported {categories.Length}";
        }

        //Query 2
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            var xmlSerializer = new XmlSerializer(typeof(ImportUserDTO[]), new XmlRootAttribute("Users"));

            ImportUserDTO[] usersDTO;

            using (var reader = new StringReader(inputXml))
            {
                usersDTO = (ImportUserDTO[])xmlSerializer.Deserialize(reader);
            }

            var users = Mapper.Map<User[]>(usersDTO);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully impported {users.Length}";
        }

        //Query 3
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            var xmlSerializer = new XmlSerializer(typeof(ImportProductDTO[]), new XmlRootAttribute("Products"));

            ImportProductDTO[] productDTOs;

            using (var stream = new StringReader(inputXml))
            {
                productDTOs = (ImportProductDTO[])xmlSerializer.Deserialize(stream);
            }

            var products = Mapper.Map<Product[]>(productDTOs)
                .Where(x => context.Users.Any(u => u.Id == x.SellerId))
                .ToArray();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully impported {products.Length}";

        }

        //Query 4
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCategoryProductsDTO[]), new XmlRootAttribute("CategoryProducts"));

            ImportCategoryProductsDTO[] categoryProductsDTOs;

            using (var stream = new StringReader(inputXml))
            {
                categoryProductsDTOs = (ImportCategoryProductsDTO[])serializer.Deserialize(stream);
            }

            var categoryProducts = Mapper.Map<CategoryProduct[]>(categoryProductsDTOs)
                .Where(x => context.Categories.Any(c => c.Id == x.CategoryId) && context.Products.Any(p => p.Id == x.ProductId))
                .ToArray();

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully impported {categoryProducts.Length}";

        }

        //Query 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderByDescending(x => x.Price)
                .Take(10)
                .ProjectTo<ProductNameAndPriceDTO>()
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<ProductNameAndPriceDTO>), new XmlRootAttribute("Products"));

            var xmlText = String.Empty;

            using FileStream stream = File.OpenWrite(destinationPath + "/prodcuts-in-range.xml");
            using var stringWriter = new StringWriter();

            xmlSerializer.Serialize(stream, products);
            xmlSerializer.Serialize(stringWriter, products);

            xmlText = stringWriter.ToString();


            return xmlText;
        }

        //Query 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                //.Select(u => new UserProductsDTO
                //{
                //    FirstName = u.FirstName,
                //    LastName = u.LastName,
                //    SoldProducts = u.ProductsSold
                //    .Select(p => new ProductNameAndPriceDTO
                //    {
                //        Name = p.Name,
                //        Price = p.Price,
                //    })
                //    .ToArray()
                //})
                .ProjectTo<UserProductsDTO>()
                .ToList();

            var serializer = new XmlSerializer(typeof(List<UserProductsDTO>), new XmlRootAttribute("Users"));

            var xmlResult = String.Empty;

            using FileStream fWriter = File.OpenWrite(destinationPath + "users-sold-products.xml");
            using StringWriter sWriter = new StringWriter();

            serializer.Serialize(fWriter, products);
            serializer.Serialize(sWriter, products);

            xmlResult = sWriter.ToString();


            return xmlResult;
        }

        //Query 7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new CategoryProductsDTO
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price),
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CategoryProductsDTO[]), new XmlRootAttribute("Categories"));

            using FileStream fWriter = File.OpenWrite(destinationPath + "categories-by-products.xml");
            using StringWriter stringWriter = new StringWriter();

            serializer.Serialize(fWriter, categories);
            serializer.Serialize(stringWriter, categories);

            var result = stringWriter.ToString();

            return result;
        }

        //Query 8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any())
                .Select(u => new UserFullInfoDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsDTO
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold
                            .Select(p => new ProductNameAndPriceDTO
                            {
                                Name = p.Name,
                                Price = p.Price,
                            })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .Take(10)
                .ToArray();

            var userCountDTO = new UsersCountDTO
            {
                Count = users.Length,
                Users = users
            };

            using FileStream fwriter = File.OpenWrite(destinationPath + "users-and-products.xml");
            using StringWriter sw = new StringWriter();

            var serializer = new XmlSerializer(typeof(UsersCountDTO), new XmlRootAttribute("Users"));

            serializer.Serialize(fwriter, userCountDTO);
            serializer.Serialize(sw, userCountDTO);

            return sw.ToString();
        }
    }
}