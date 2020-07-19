using System;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTO.Category;
using ProductShop.DTO.Users;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            InitializeMapper();

            var jsonResult = GetUsersWithProducts(context);

            File.WriteAllText("../../../Datasets/JsonResults/users-and-products.json", jsonResult);
        }

        private static void ResetDatabase(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            Console.WriteLine("Database was sucessfully deleted!");
            context.Database.EnsureCreated();
            Console.WriteLine("Database was sucessfully created");


            var inputJson = File.ReadAllText("../../../Datasets/users.json");
            Console.WriteLine(ImportUsers(context, inputJson));
            inputJson = File.ReadAllText("../../../Datasets/products.json");
            Console.WriteLine(ImportProducts(context, inputJson));
            inputJson = File.ReadAllText("../../../Datasets/categories.json");
            Console.WriteLine(ImportCategories(context, inputJson));
            inputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            Console.WriteLine(ImportCategoryProducts(context, inputJson));
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }); 
        }

        //Problem 2
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        //Problem 3
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        //Problem 4
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson)
                .Where(x => x.Name != default)
                .ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        //Problem 5
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryproducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoryProducts.AddRange(categoryproducts);
            context.SaveChanges();

            return $"Successfully imported {categoryproducts.Length}";
        }

        //Problem 6
        public static string GetProductsInRange(ProductShopContext context)
        {
            var startRange = 500;
            var endRange = 1000;

            var productsInRange = context.Products
                .Where(x => x.Price >= startRange && x.Price <= endRange)
                .Select(x => new
                {
                    x.Name,
                    x.Price,
                    Seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .OrderBy(x => x.Price)
                .ToList();

            var contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(productsInRange);

            return json;
        }

        //Problem 7
        public static string GetSoldProducts(ProductShopContext context)
        {
            var soldProducts = context.Users
                .Where(x => x.ProductsSold.Any(p => p.Buyer != default))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.LastName)
                //.Select(u => new
                //{
                //    firstName = u.FirstName,
                //    lastName = u.LastName,
                //    soldProducts = u.ProductsSold
                //                    .Where(x => x.Buyer != default)
                //                    .Select(i => new
                //                    {
                //                        name = i.Name,
                //                        price = i.Price,
                //                        buyerFirstName = i.Buyer.FirstName,
                //                        buyerLastName = i.Buyer.LastName
                //                    })
                //                    .ToArray()
                //})
                .ProjectTo<UsersSoldItemsDTO>()
                .ToArray();

            var jsonResult = JsonConvert.SerializeObject(soldProducts, Formatting.Indented);

            return jsonResult;
        }

        //Problem 8 
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categoriesInfo = context.Categories
                .OrderByDescending(x => x.CategoryProducts.Count)
                //.Select(x => new
                //{
                //    category = x.Name,
                //    productCount = x.CategoryProducts.Count,
                //    averagePrice = x.CategoryProducts.Average(p => p.Product.Price).ToString("F2"),
                //    totalRevenue = x.CategoryProducts.Sum(p => p.Product.Price).ToString("F2")
                //})
                .ProjectTo<CategoryInformationDTO>()
                .ToArray();

            var jsonResult = JsonConvert.SerializeObject(categoriesInfo, Formatting.Indented);

            return jsonResult;
        }

        //Problem 9
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != default))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count(),
                        products = u.ProductsSold
                        .Where(p => p.Buyer != default)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                    }
                })
                //.ProjectTo<UserCountCollectionDTO>()
                .OrderByDescending(u => u.soldProducts.count)
                .ToArray();

            var usersObject = new
            {
                usersCount = users.Count(),
                users = users
            };

            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var jsonResult = JsonConvert.SerializeObject(usersObject, jsonSettings);

            return jsonResult;
        }
    }
}