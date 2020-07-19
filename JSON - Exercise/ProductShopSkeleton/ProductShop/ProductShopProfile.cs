using AutoMapper;
using ProductShop.DTO.Category;
using ProductShop.DTO.Product;
using ProductShop.DTO.Users;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<Product, ProductDTO>()
                .ForMember(x => x.BuyerFirstName,
                y => y.MapFrom(x => x.Buyer.FirstName))
                .ForMember(x => x.BuyerLastName,
                y => y.MapFrom(x => x.Buyer.LastName));

            this.CreateMap<User, UsersSoldItemsDTO>()
                .ForMember(x => x.SoldProducts,
                y => y.MapFrom(x => x.ProductsSold.Where(p => p.Buyer != default)));

            this.CreateMap<Category, CategoryInformationDTO>()
                .ForMember(x => x.CategoryName,
                y => y.MapFrom(x => x.Name))
                .ForMember(x => x.ProductsCount,
                y => y.MapFrom(x => x.CategoryProducts.Count()))
                .ForMember(x => x.AveragePrice,
                y => y.MapFrom(x => x.CategoryProducts.Average(p => p.Product.Price).ToString("F2")))
                .ForMember(x => x.TotalRevenue,
                y => y.MapFrom(x => x.CategoryProducts.Sum(p => p.Product.Price).ToString("F2")));

            //Problem 9
            this.CreateMap<Product, ProductNamePriceDTO>();

            this.CreateMap<ProductCollectionDTO, ProductCollectionDTO>()
                .ForMember(x => x.Count,
                y => y.MapFrom(x => x.Products.Length));

            this.CreateMap<User, UserSoldProductsDTO>();

            this.CreateMap<UserCountCollectionDTO, UserCountCollectionDTO>()
                .ForMember(x => x.UsersCount,
                y => y.MapFrom(x => x.Users.Length));
        }
    }
}
