using System.ComponentModel.DataAnnotations;

namespace BookShop.DataProcessor.ImportDto
{
    public class ImportAuthorBooksDto
    {
        [Required]
        public int? Id { get; set; }
    }
}