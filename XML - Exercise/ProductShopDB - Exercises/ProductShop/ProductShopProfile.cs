using AutoMapper;
using ProductShop.Dtos.Export.Product;
using ProductShop.Dtos.Export.Users;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Import Categories
            this.CreateMap<ImportCategoryDTO, Category>();

            //Import Users
            this.CreateMap<ImportUserDTO, User>();

            //Import Product
            this.CreateMap<ImportProductDTO, Product>();

            //Import CategoryProducts
            this.CreateMap<ImportCategoryProductsDTO, CategoryProduct>();


            //Query exercises

            //Quert 5.
            this.CreateMap<Product, ProductNameAndPriceDTO>();

            //Query 6
            this.CreateMap<User, UserProductsDTO>()
                .ForMember(x => x.SoldProducts,
                 y => y.MapFrom(x => x.ProductsSold
                                    .Where(ps => ps.Buyer != null)
                                    .Select(p => Mapper.Map<ProductNameAndPriceDTO>(p))));
        }
    }
}
