using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportEmployeeDto
    {
        public ImportEmployeeDto()
        {
            this.TaskIds = new int[0];
        }

        [Required]
        [MinLength(3), MaxLength(40)]
        [RegularExpression("^[A-za-z0-9]+$")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(\d{3})-(\d{3})-(\d{4})$")]
        public string Phone { get; set; }

        [JsonProperty("Tasks")]
        public int[] TaskIds { get; set; }
    }
}
