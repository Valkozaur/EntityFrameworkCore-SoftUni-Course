using ProductShop.Dtos.Export.Product;

namespace ProductShop.Dtos.Export.Users.UserDetaildInterfaces
{
    public interface ISoldProductDetails
    {
        ProductNameAndPriceDTO[] SoldProducts { get; set; }
    }
}
