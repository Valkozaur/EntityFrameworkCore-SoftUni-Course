namespace PetStore.Data.Models
{
    public class OrderMerchandise
    {
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }

        public string MerchandiseId { get; set; }
        public virtual Merchandise Merchandise { get; set; }
    }
}